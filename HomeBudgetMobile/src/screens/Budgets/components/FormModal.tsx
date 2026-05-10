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
import { budgetsApi } from "../../../api/apiService";
import { BudgetDto, CategoryDto } from "../../../models/types";
import { colors, getMonthName } from "../../../utils/helpers";
import { fm } from "../../../styles/Budgets";

const HOUSEHOLD_ID = 1;
const USER_ID = 1;

export default function FormModal({
  visible,
  onClose,
  onSave,
  editing,
  categories,
  month,
  year,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: BudgetDto | null;
  categories: CategoryDto[];
  month: number;
  year: number;
}) {
  const [amount, setAmount] = useState("");
  const [categoryId, setCategoryId] = useState(0);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      if (editing) {
        setAmount(editing.amount.toString());
        setCategoryId(editing.categoryId);
      } else {
        setAmount("");
        setCategoryId(categories[0]?.id ?? 0);
      }
    }
  }, [visible, editing, categories]);

  const handleSave = async () => {
    if (!amount || !categoryId) {
      Alert.alert("Błąd", "Uzupełnij wszystkie pola");
      return;
    }
    setSaving(true);
    try {
      if (editing) {
        await budgetsApi.update(editing.id, parseFloat(amount.replace(",", ".")));
      } else {
        await budgetsApi.create({
          amount: parseFloat(amount.replace(",", ".")),
          month, year, userId: USER_ID, categoryId, householdId: HOUSEHOLD_ID,
        });
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać budżetu");
    } finally {
      setSaving(false);
    }
  };

  const expenseCats = categories.filter(
    (c) => c.type === "Expense" || c.type === "Both"
  );

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
            {editing ? "Edytuj budżet" : "Nowy budżet"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && { opacity: 0.5 }]}>Zapisz</Text>
          </TouchableOpacity>
        </View>
        <ScrollView style={fm.body} keyboardShouldPersistTaps="handled">
          <Text style={fm.label}>Miesiąc</Text>
          <View style={fm.infoBox}>
            <Text style={fm.infoText}>
              {getMonthName(month)} {year}
            </Text>
          </View>

          {!editing && (
            <>
              <Text style={fm.label}>Kategoria *</Text>
              <ScrollView
                horizontal
                showsHorizontalScrollIndicator={false}
                style={fm.scrollSpacer}
              >
                <View style={fm.row}>
                  {expenseCats.map((c) => (
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

          <Text style={fm.label}>Limit (zł) *</Text>
          <TextInput
            style={fm.input}
            value={amount}
            onChangeText={setAmount}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
          />
        </ScrollView>
      </KeyboardAvoidingView>
    </Modal>
  );
}
