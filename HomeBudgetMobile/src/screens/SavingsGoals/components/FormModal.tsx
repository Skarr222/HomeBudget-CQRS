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
import { savingsGoalsApi } from "../../../api/apiService";
import { SavingsGoalDto, UpdateSavingsGoalDto } from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/SavingsGoals";
import {
  PRESET_COLORS,
  PRESET_ICONS,
} from "../../../utils/SavingsGoals/constants";

const HOUSEHOLD_ID = 1;

export default function FormModal({
  visible,
  onClose,
  onSave,
  editing,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: SavingsGoalDto | null;
}) {
  const [name, setName] = useState("");
  const [target, setTarget] = useState("");
  const [current, setCurrent] = useState("0");
  const [deadline, setDeadline] = useState("");
  const [icon, setIcon] = useState("savings");
  const [color, setColor] = useState("#6366F1");
  const [isCompleted, setIsCompleted] = useState(false);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      if (editing) {
        setName(editing.name);
        setTarget(editing.targetAmount.toString());
        setCurrent(editing.currentAmount.toString());
        setDeadline(editing.deadline ? editing.deadline.slice(0, 10) : "");
        setIcon(editing.icon);
        setColor(editing.color);
        setIsCompleted(editing.isCompleted);
      } else {
        setName("");
        setTarget("");
        setCurrent("0");
        setDeadline("");
        setIcon("savings");
        setColor("#6366F1");
        setIsCompleted(false);
      }
    }
  }, [visible, editing]);

  const handleSave = async () => {
    if (!name.trim() || !target) {
      Alert.alert("Błąd", "Uzupełnij wymagane pola");
      return;
    }
    setSaving(true);
    try {
      if (editing) {
        const dto: UpdateSavingsGoalDto = {
          id: editing.id,
          name: name.trim(),
          targetAmount: parseFloat(target.replace(",", ".")),
          currentAmount: parseFloat(current.replace(",", ".") || "0"),
          deadline: deadline || null,
          icon,
          color,
          isCompleted,
        };
        await savingsGoalsApi.update(editing.id, dto);
      } else {
        await savingsGoalsApi.create({
          name: name.trim(),
          targetAmount: parseFloat(target.replace(",", ".")),
          deadline: deadline || undefined,
          icon,
          color,
          householdId: HOUSEHOLD_ID,
        });
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać");
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
            {editing ? "Edytuj cel" : "Nowy cel"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && fm.savingOpacity]}>Zapisz</Text>
          </TouchableOpacity>
        </View>
        <ScrollView style={fm.body} keyboardShouldPersistTaps="handled">
          <Text style={fm.label}>Nazwa *</Text>
          <TextInput
            style={fm.input}
            value={name}
            onChangeText={setName}
            placeholder="np. Wakacje 2025"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Cel (zł) *</Text>
          <TextInput
            style={fm.input}
            value={target}
            onChangeText={setTarget}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
          />

          {editing && (
            <>
              <Text style={fm.label}>Zebrano (zł)</Text>
              <TextInput
                style={fm.input}
                value={current}
                onChangeText={setCurrent}
                keyboardType="decimal-pad"
                placeholder="0.00"
                placeholderTextColor={colors.textMuted}
              />
            </>
          )}

          <Text style={fm.label}>Deadline</Text>
          <TextInput
            style={fm.input}
            value={deadline}
            onChangeText={setDeadline}
            placeholder="DD-MM-YYYY"
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

          <Text style={fm.label}>Ikona</Text>
          <ScrollView horizontal showsHorizontalScrollIndicator={false}>
            <View style={fm.row}>
              {PRESET_ICONS.map((ic) => (
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
                  {isCompleted ? "Cel osiągnięty" : "W trakcie"}
                </Text>
              </TouchableOpacity>
            </>
          )}
        </ScrollView>
      </KeyboardAvoidingView>
    </Modal>
  );
}
