import React, { useState } from "react";
import {
  View,
  Text,
  Modal,
  TextInput,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  Alert,
} from "react-native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { accountsApi } from "../../../api/apiService";
import {
  AccountDto,
  AccountType,
  CreateAccountDto,
  UpdateAccountDto,
} from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/Accounts";
import {
  ACCOUNT_TYPES,
  PRESET_COLORS,
} from "../../../utils/Accounts/constants";

const USER_ID = 1;

export default function FormModal({
  visible,
  onClose,
  onSave,
  editing,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: AccountDto | null;
}) {
  const [name, setName] = useState("");
  const [type, setType] = useState<AccountType>(AccountType.Checking);
  const [balance, setBalance] = useState("");
  const [color, setColor] = useState("#6366F1");
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      if (editing) {
        setName(editing.name);
        setType(editing.type);
        setBalance(editing.balance.toString());
        setColor(editing.color);
      } else {
        setName("");
        setType(AccountType.Checking);
        setBalance("0");
        setColor("#6366F1");
      }
    }
  }, [visible, editing]);

  const handleSave = async () => {
    if (!name.trim()) {
      Alert.alert("Błąd", "Podaj nazwę konta");
      return;
    }
    setSaving(true);
    try {
      if (editing) {
        const dto: UpdateAccountDto = {
          id: editing.id,
          name: name.trim(),
          type,
          balance: parseFloat(balance.replace(",", ".") || "0"),
          color,
          icon:
            ACCOUNT_TYPES.find((t) => t.value === type)?.icon ??
            "account-balance",
        };
        await accountsApi.update(editing.id, dto);
      } else {
        const dto: CreateAccountDto = {
          name: name.trim(),
          type,
          balance: parseFloat(balance.replace(",", ".") || "0"),
          color,
          icon:
            ACCOUNT_TYPES.find((t) => t.value === type)?.icon ??
            "account-balance",
          userId: USER_ID,
        };
        await accountsApi.create(dto);
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać konta");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal
      visible={visible}
      animationType="slide"
      presentationStyle="pageSheet"
      onRequestClose={onClose}
    >
      <KeyboardAvoidingView
        style={fm.flex1}
        behavior={Platform.OS === "ios" ? "padding" : undefined}
      >
        <View style={fm.header}>
          <TouchableOpacity onPress={onClose}>
            <Text style={fm.cancel}>Anuluj</Text>
          </TouchableOpacity>
          <Text style={fm.headerTitle}>
            {editing ? "Edytuj konto" : "Nowe konto"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && fm.savingOpacity]}>Zapisz</Text>
          </TouchableOpacity>
        </View>
        <View style={fm.body}>
          <Text style={fm.label}>Nazwa *</Text>
          <TextInput
            style={fm.input}
            value={name}
            onChangeText={setName}
            placeholder="np. PKO BP"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Typ konta</Text>
          <View style={fm.typeGrid}>
            {ACCOUNT_TYPES.map((t) => (
              <TouchableOpacity
                key={t.value}
                style={[fm.typeBtn, type === t.value && fm.typeBtnActive]}
                onPress={() => setType(t.value)}
              >
                <MaterialIcons
                  name={t.icon as any}
                  size={20}
                  color={type === t.value ? "#fff" : colors.textSecondary}
                />
                <Text
                  style={[
                    fm.typeBtnText,
                    type === t.value && fm.typeBtnTextSelected,
                  ]}
                >
                  {t.label}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={fm.label}>Saldo (zł)</Text>
          <TextInput
            style={fm.input}
            value={balance}
            onChangeText={setBalance}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Kolor</Text>
          <View style={fm.colorRow}>
            {PRESET_COLORS.map((c) => (
              <TouchableOpacity
                key={c}
                onPress={() => setColor(c)}
                style={[
                  fm.colorDot,
                  { backgroundColor: c },
                  color === c && fm.colorDotActive,
                ]}
              />
            ))}
          </View>
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}
