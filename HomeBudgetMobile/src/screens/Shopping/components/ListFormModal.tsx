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
import { shoppingListsApi } from "../../../api/apiService";
import { ShoppingListDto } from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/Shopping";

const HOUSEHOLD_ID = 1;
const USER_ID = 1;

export default function ListFormModal({
  visible,
  onClose,
  onSave,
  editing,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: ShoppingListDto | null;
}) {
  const [name, setName] = useState("");
  const [isCompleted, setIsCompleted] = useState(false);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      setName(editing ? editing.name : "");
      setIsCompleted(editing ? editing.isCompleted : false);
    }
  }, [visible, editing]);

  const handleSave = async () => {
    if (!name.trim()) {
      Alert.alert("Błąd", "Podaj nazwę listy");
      return;
    }
    setSaving(true);
    try {
      if (editing) {
        await shoppingListsApi.update(editing.id, name.trim(), isCompleted);
      } else {
        await shoppingListsApi.create(name.trim(), USER_ID, HOUSEHOLD_ID);
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać listy");
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
            {editing ? "Edytuj listę" : "Nowa lista"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && fm.savingOpacity]}>Zapisz</Text>
          </TouchableOpacity>
        </View>
        <View style={fm.body}>
          <Text style={fm.label}>Nazwa listy *</Text>
          <TextInput
            style={fm.input}
            value={name}
            onChangeText={setName}
            placeholder="np. Biedronka środa"
            placeholderTextColor={colors.textMuted}
            autoFocus
          />
          {editing && (
            <>
              <Text style={[fm.label, fm.labelMarginTop]}>Status</Text>
              <TouchableOpacity
                style={[fm.toggleBtn, isCompleted && fm.toggleBtnActive]}
                onPress={() => setIsCompleted(!isCompleted)}
              >
                <MaterialIcons
                  name={isCompleted ? "check-circle" : "radio-button-unchecked"}
                  size={20}
                  color={isCompleted ? "#fff" : colors.textSecondary}
                />
                <Text style={[fm.toggleText, isCompleted && fm.toggleTextSelected]}>
                  {isCompleted ? "Zakończona" : "Aktywna"}
                </Text>
              </TouchableOpacity>
            </>
          )}
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}
