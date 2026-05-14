import React, { useState } from "react";
import {
  View,
  Text,
  Modal,
  TextInput,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  Alert,
} from "react-native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { billsApi } from "../../../api/apiService";
import { BillDto, CategoryDto, UpdateBillDto } from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/Bills";
import { PRESET_COLORS, BILL_ICONS } from "../../../utils/Bills/constants";

const HOUSEHOLD_ID = 1;

export default function FormModal({
  visible,
  onClose,
  onSave,
  editing,
  categories,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: BillDto | null;
  categories: CategoryDto[];
}) {
  const [name, setName] = useState("");
  const [provider, setProvider] = useState("");
  const [dueDay, setDueDay] = useState("1");
  const [amount, setAmount] = useState("");
  const [categoryId, setCategoryId] = useState(0);
  const [icon, setIcon] = useState("receipt");
  const [color, setColor] = useState("#F59E0B");
  const [isActive, setIsActive] = useState(true);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      if (editing) {
        setName(editing.name);
        setProvider(editing.provider);
        setDueDay(editing.dueDay.toString());
        setAmount(editing.estimatedAmount.toString());
        setCategoryId(editing.categoryId);
        setIcon(editing.icon);
        setColor(editing.color);
        setIsActive(editing.isActive);
      } else {
        setName("");
        setProvider("");
        setDueDay("1");
        setAmount("");
        setCategoryId(categories[0]?.id ?? 0);
        setIcon("receipt");
        setColor("#F59E0B");
        setIsActive(true);
      }
    }
  }, [visible, editing, categories]);

  const handleSave = async () => {
    if (!name.trim() || !amount || !categoryId) {
      Alert.alert("Błąd", "Uzupełnij wymagane pola");
      return;
    }
    const day = parseInt(dueDay);
    if (isNaN(day) || day < 1 || day > 31) {
      Alert.alert("Błąd", "Dzień płatności: 1-31");
      return;
    }
    setSaving(true);
    try {
      if (editing) {
        const dto: UpdateBillDto = {
          id: editing.id,
          name: name.trim(),
          provider,
          dueDay: day,
          estimatedAmount: parseFloat(amount.replace(",", ".")),
          icon,
          color,
          isActive,
        };
        await billsApi.update(editing.id, dto);
      } else {
        await billsApi.create({
          name: name.trim(),
          provider,
          dueDay: day,
          estimatedAmount: parseFloat(amount.replace(",", ".")),
          icon,
          color,
          householdId: HOUSEHOLD_ID,
          categoryId,
        });
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać rachunku");
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
        style={{ flex: 1 }}
        behavior={Platform.OS === "ios" ? "padding" : undefined}
      >
        <View style={fm.header}>
          <TouchableOpacity onPress={onClose}>
            <Text style={fm.cancel}>Anuluj</Text>
          </TouchableOpacity>
          <Text style={fm.headerTitle}>
            {editing ? "Edytuj rachunek" : "Nowy rachunek"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && { opacity: 0.5 }]}>Zapisz</Text>
          </TouchableOpacity>
        </View>
        <ScrollView style={fm.body} keyboardShouldPersistTaps="handled">
          <Text style={fm.label}>Nazwa *</Text>
          <TextInput
            style={fm.input}
            value={name}
            onChangeText={setName}
            placeholder="np. Prąd"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Dostawca</Text>
          <TextInput
            style={fm.input}
            value={provider}
            onChangeText={setProvider}
            placeholder="np. Enea"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Dzień płatności (1-31) *</Text>
          <TextInput
            style={fm.input}
            value={dueDay}
            onChangeText={setDueDay}
            keyboardType="number-pad"
            placeholder="1"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Szacowana kwota (zł) *</Text>
          <TextInput
            style={[fm.input, editing?.paidThisMonth && { opacity: 0.4 }]}
            value={amount}
            onChangeText={setAmount}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
            editable={!editing?.paidThisMonth}
          />
          {editing?.paidThisMonth && (
            <Text style={{ fontSize: 11, color: colors.textMuted, marginBottom: 4 }}>
              Kwota zablokowana — rachunek opłacony w tym miesiącu
            </Text>
          )}

          {!editing && (
            <>
              <Text style={fm.label}>Kategoria *</Text>
              <ScrollView
                horizontal
                showsHorizontalScrollIndicator={false}
                style={fm.scrollSpacer}
              >
                <View style={fm.row}>
                  {categories
                    .filter((c) => c.type === "Expense" || c.type === "Both")
                    .map((c) => (
                      <TouchableOpacity
                        key={c.id}
                        style={[
                          fm.catChip,
                          categoryId === c.id && {
                            backgroundColor: c.color,
                            borderColor: c.color,
                          },
                        ]}
                        onPress={() => setCategoryId(c.id)}
                      >
                        <Text
                          style={[
                            fm.chipText,
                            categoryId === c.id && { color: "#fff" },
                          ]}
                        >
                          {c.name}
                        </Text>
                      </TouchableOpacity>
                    ))}
                </View>
              </ScrollView>
            </>
          )}

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

          <Text style={fm.label}>Ikona</Text>
          <ScrollView horizontal showsHorizontalScrollIndicator={false}>
            <View style={fm.row}>
              {BILL_ICONS.map((ic) => (
                <TouchableOpacity
                  key={ic}
                  style={[
                    fm.iconChip,
                    icon === ic && {
                      backgroundColor: color,
                      borderColor: color,
                    },
                  ]}
                  onPress={() => setIcon(ic)}
                >
                  <MaterialIcons
                    name={ic as any}
                    size={20}
                    color={icon === ic ? "#fff" : colors.textSecondary}
                  />
                </TouchableOpacity>
              ))}
            </View>
          </ScrollView>

          {editing && (
            <>
              <Text style={[fm.label, { marginTop: 16 }]}>Status</Text>
              <TouchableOpacity
                style={[fm.toggleBtn, isActive ? fm.toggleBtnActive : {}]}
                onPress={() => setIsActive(!isActive)}
              >
                <MaterialIcons
                  name={isActive ? "check-circle" : "cancel"}
                  size={20}
                  color={isActive ? "#fff" : colors.textMuted}
                />
                <Text style={[fm.toggleText, isActive && { color: "#fff" }]}>
                  {isActive ? "Aktywny" : "Nieaktywny"}
                </Text>
              </TouchableOpacity>
            </>
          )}
        </ScrollView>
      </KeyboardAvoidingView>
    </Modal>
  );
}
