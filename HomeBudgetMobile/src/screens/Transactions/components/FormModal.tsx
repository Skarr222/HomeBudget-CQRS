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
import { transactionsApi } from "../../../api/apiService";
import {
  AccountDto,
  CategoryDto,
  CreateTransactionDto,
  TransactionDto,
  TransactionType,
  UpdateTransactionDto,
} from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/Transactions";
import { PAYMENT_METHODS } from "../../../utils/Transactions/constants";

const HOUSEHOLD_ID = 1;
const USER_ID = 1;

export default function FormModal({
  visible,
  onClose,
  onSave,
  editing,
  categories,
  accounts,
}: {
  visible: boolean;
  onClose: () => void;
  onSave: () => void;
  editing: TransactionDto | null;
  categories: CategoryDto[];
  accounts: AccountDto[];
}) {
  const [title, setTitle] = useState("");
  const [amount, setAmount] = useState("");
  const [date, setDate] = useState(new Date().toISOString().slice(0, 10));
  const [note, setNote] = useState("");
  const [type, setType] = useState<TransactionType>(TransactionType.Expense);
  const [method, setMethod] = useState(PAYMENT_METHODS[0]);
  const [categoryId, setCategoryId] = useState<number>(0);
  const [accountId, setAccountId] = useState<number>(0);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible) {
      if (editing) {
        setTitle(editing.title);
        setAmount(editing.amount.toString());
        setDate(editing.date.slice(0, 10));
        setNote(editing.note ?? "");
        setType(editing.type);
        setMethod(editing.paymentMethod);
        setCategoryId(editing.categoryId);
        setAccountId(editing.accountId);
      } else {
        setTitle("");
        setAmount("");
        setNote("");
        setDate(new Date().toISOString().slice(0, 10));
        setType(TransactionType.Expense);
        setMethod(PAYMENT_METHODS[0]);
        setCategoryId(categories[0]?.id ?? 0);
        setAccountId(accounts[0]?.id ?? 0);
      }
    }
  }, [visible, editing, categories, accounts]);

  const handleSave = async () => {
    if (!title.trim() || !amount || !categoryId || !accountId) {
      Alert.alert("Błąd", "Uzupełnij wymagane pola");
      return;
    }
    setSaving(true);
    try {
      const parsedAmount = parseFloat(amount.replace(",", "."));
      if (editing) {
        const dto: UpdateTransactionDto = {
          id: editing.id,
          title: title.trim(),
          amount: parsedAmount,
          date: new Date(date).toISOString(),
          note: note || undefined,
          type,
          paymentMethod: method,
          isShared: false,
          categoryId,
          accountId,
        };
        await transactionsApi.update(editing.id, dto);
      } else {
        const dto: CreateTransactionDto = {
          title: title.trim(),
          amount: parsedAmount,
          date: new Date(date).toISOString(),
          note: note || undefined,
          type,
          paymentMethod: method,
          isShared: false,
          userId: USER_ID,
          categoryId,
          accountId,
          householdId: HOUSEHOLD_ID,
        };
        await transactionsApi.create(dto);
      }
      onSave();
    } catch {
      Alert.alert("Błąd", "Nie udało się zapisać transakcji");
    } finally {
      setSaving(false);
    }
  };

  const filteredCats = categories.filter((c) =>
    type === TransactionType.Income
      ? c.type === "Income" || c.type === "Both"
      : c.type === "Expense" || c.type === "Both"
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
            {editing ? "Edytuj" : "Nowa transakcja"}
          </Text>
          <TouchableOpacity onPress={handleSave} disabled={saving}>
            <Text style={[fm.save, saving && { opacity: 0.5 }]}>Zapisz</Text>
          </TouchableOpacity>
        </View>

        <ScrollView style={fm.body} keyboardShouldPersistTaps="handled">
          <Text style={fm.label}>Typ</Text>
          <View style={fm.row}>
            {(
              [
                TransactionType.Expense,
                TransactionType.Income,
              ] as TransactionType[]
            ).map((t) => (
              <TouchableOpacity
                key={t}
                style={[fm.chip, type === t && fm.chipActive]}
                onPress={() => {
                  setType(t);
                  setCategoryId(0);
                }}
              >
                <Text style={[fm.chipText, type === t && fm.chipTextActive]}>
                  {t === TransactionType.Expense ? "Wydatek" : "Przychód"}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={fm.label}>Tytuł *</Text>
          <TextInput
            style={fm.input}
            value={title}
            onChangeText={setTitle}
            placeholder="np. Zakupy Biedronka"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Kwota (zł) *</Text>
          <TextInput
            style={fm.input}
            value={amount}
            onChangeText={setAmount}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Data</Text>
          <TextInput
            style={fm.input}
            value={date}
            onChangeText={setDate}
            placeholder="RRRR-MM-DD"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Metoda płatności</Text>
          <ScrollView
            horizontal
            showsHorizontalScrollIndicator={false}
            style={fm.scrollSpacer}
          >
            <View style={fm.row}>
              {PAYMENT_METHODS.map((m) => (
                <TouchableOpacity
                  key={m}
                  style={[fm.chip, method === m && fm.chipActive]}
                  onPress={() => setMethod(m)}
                >
                  <Text
                    style={[fm.chipText, method === m && fm.chipTextActive]}
                  >
                    {m}
                  </Text>
                </TouchableOpacity>
              ))}
            </View>
          </ScrollView>

          <Text style={fm.label}>Kategoria *</Text>
          <ScrollView
            horizontal
            showsHorizontalScrollIndicator={false}
            style={fm.scrollSpacer}
          >
            <View style={fm.row}>
              {filteredCats.map((c) => (
                <TouchableOpacity
                  key={c.id}
                  style={[
                    fm.catChip,
                    categoryId === c.id && { backgroundColor: c.color },
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

          <Text style={fm.label}>Konto *</Text>
          <View style={fm.row}>
            {accounts.map((a) => (
              <TouchableOpacity
                key={a.id}
                style={[fm.chip, accountId === a.id && fm.chipActive]}
                onPress={() => setAccountId(a.id)}
              >
                <Text
                  style={[fm.chipText, accountId === a.id && fm.chipTextActive]}
                >
                  {a.name}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={fm.label}>Notatka</Text>
          <TextInput
            style={[fm.input, { height: 80, textAlignVertical: "top" }]}
            value={note}
            onChangeText={setNote}
            multiline
            placeholder="Opcjonalna notatka..."
            placeholderTextColor={colors.textMuted}
          />
        </ScrollView>
      </KeyboardAvoidingView>
    </Modal>
  );
}
