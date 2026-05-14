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
import { billsApi } from "../../../api/apiService";
import { AccountDto, BillDto, PayBillDto } from "../../../models/types";
import { PaymentMethod } from "../../../models/enums";
import { colors } from "../../../utils/helpers";
import { fm } from "../../../styles/Bills";

const USER_ID = 1;

const PAYMENT_METHODS: { label: string; value: PaymentMethod }[] = [
  { label: "Karta", value: PaymentMethod.Card },
  { label: "Gotówka", value: PaymentMethod.Cash },
  { label: "BLIK", value: PaymentMethod.BLIK },
  { label: "Przelew", value: PaymentMethod.Transfer },
];

export default function PayModal({
  visible,
  onClose,
  onPaid,
  bill,
  accounts,
}: {
  visible: boolean;
  onClose: () => void;
  onPaid: () => void;
  bill: BillDto | null;
  accounts: AccountDto[];
}) {
  const [amount, setAmount] = useState("");
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(
    PaymentMethod.Card
  );
  const [accountId, setAccountId] = useState(0);
  const [saving, setSaving] = useState(false);

  React.useEffect(() => {
    if (visible && bill) {
      setAmount(bill.estimatedAmount.toString());
      setPaymentMethod(PaymentMethod.Card);
      setAccountId(accounts[0]?.id ?? 0);
    }
  }, [visible, bill, accounts]);

  const handlePay = async () => {
    if (!bill) return;
    const parsed = parseFloat(amount.replace(",", "."));
    if (isNaN(parsed) || parsed <= 0) {
      Alert.alert("Błąd", "Podaj poprawną kwotę");
      return;
    }
    if (!accountId) {
      Alert.alert("Błąd", "Wybierz konto");
      return;
    }
    setSaving(true);
    try {
      const dto: PayBillDto = {
        billId: bill.id,
        amount: parsed,
        paymentMethod,
        accountId,
        userId: USER_ID,
        paidDate: new Date().toISOString(),
      };
      await billsApi.pay(bill.id, dto);
      onPaid();
    } catch {
      Alert.alert("Błąd", "Nie udało się opłacić rachunku");
    } finally {
      setSaving(false);
    }
  };

  if (!bill) return null;

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
          <Text style={fm.headerTitle}>Opłać: {bill.name}</Text>
          <TouchableOpacity onPress={handlePay} disabled={saving}>
            <Text style={[fm.save, saving && { opacity: 0.5 }]}>Opłać</Text>
          </TouchableOpacity>
        </View>

        <ScrollView style={fm.body} keyboardShouldPersistTaps="handled">
          <Text style={fm.label}>Kwota (zł) *</Text>
          <TextInput
            style={fm.input}
            value={amount}
            onChangeText={setAmount}
            keyboardType="decimal-pad"
            placeholder="0.00"
            placeholderTextColor={colors.textMuted}
          />

          <Text style={fm.label}>Metoda płatności</Text>
          <View style={fm.row}>
            {PAYMENT_METHODS.map((pm) => (
              <TouchableOpacity
                key={pm.value}
                style={[
                  fm.catChip,
                  paymentMethod === pm.value && {
                    backgroundColor: colors.primary,
                    borderColor: colors.primary,
                  },
                ]}
                onPress={() => setPaymentMethod(pm.value)}
              >
                <Text
                  style={[
                    fm.chipText,
                    paymentMethod === pm.value && { color: "#fff" },
                  ]}
                >
                  {pm.label}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={fm.label}>Konto *</Text>
          {accounts.map((acc) => (
            <TouchableOpacity
              key={acc.id}
              style={[
                fm.toggleBtn,
                { marginBottom: 8 },
                accountId === acc.id && {
                  backgroundColor: acc.color + "20",
                  borderColor: acc.color,
                },
              ]}
              onPress={() => setAccountId(acc.id)}
            >
              <View
                style={{
                  width: 12,
                  height: 12,
                  borderRadius: 6,
                  backgroundColor: acc.color,
                }}
              />
              <Text style={[fm.toggleText, { flex: 1 }]}>{acc.name}</Text>
              <Text style={{ fontSize: 13, color: colors.textMuted }}>
                {acc.balance.toFixed(2)} zł
              </Text>
            </TouchableOpacity>
          ))}
        </ScrollView>
      </KeyboardAvoidingView>
    </Modal>
  );
}
