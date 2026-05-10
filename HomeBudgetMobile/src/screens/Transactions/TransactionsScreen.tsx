import React, { useState, useCallback } from "react";
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  RefreshControl,
  ActivityIndicator,
  Alert,
} from "react-native";
import { useFocusEffect } from "@react-navigation/native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import {
  accountsApi,
  categoriesApi,
  transactionsApi,
} from "../../api/apiService";
import {
  AccountDto,
  CategoryDto,
  TransactionDto,
  TransactionFilter,
} from "../../models/types";
import { colors, formatCurrency, formatDate } from "../../utils/helpers";
import { s } from "../../styles/Transactions";
import FormModal from "./components/FormModal";

const HOUSEHOLD_ID = 1;

export default function TransactionsScreen() {
  const [transactions, setTransactions] = useState<TransactionDto[]>([]);
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<TransactionFilter>("all");
  const [modalVisible, setModalVisible] = useState(false);
  const [editing, setEditing] = useState<TransactionDto | null>(null);

  const fetchData = async () => {
    try {
      const [txRes, catRes, accRes] = await Promise.all([
        transactionsApi.getAll({ householdId: HOUSEHOLD_ID }),
        categoriesApi.getAll(),
        accountsApi.getAll({ householdId: HOUSEHOLD_ID }),
      ]);
      setTransactions(txRes.data);
      setCategories(catRes.data);
      setAccounts(accRes.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useFocusEffect(
    useCallback(() => {
      fetchData();
    }, [])
  );

  const handleDelete = (id: number, title: string) => {
    Alert.alert("Usuń transakcję", `Usunąć "${title}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await transactionsApi.delete(id);
            setTransactions((prev) => prev.filter((t) => t.id !== id));
          } catch {
            Alert.alert("Błąd", "Nie udało się usunąć");
          }
        },
      },
    ]);
  };

  const openAdd = () => {
    setEditing(null);
    setModalVisible(true);
  };
  const openEdit = (t: TransactionDto) => {
    setEditing(t);
    setModalVisible(true);
  };

  const filtered = transactions.filter((t) => {
    if (filter === "expense") return t.type === "Expense";
    if (filter === "income") return t.type === "Income";
    return true;
  });

  const totalIncome = transactions
    .filter((t) => t.type === "Income")
    .reduce((sum, t) => sum + t.amount, 0);
  const totalExpense = transactions
    .filter((t) => t.type === "Expense")
    .reduce((sum, t) => sum + t.amount, 0);

  if (loading) {
    return (
      <View style={s.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );
  }

  return (
    <View style={s.container}>
      <View style={s.summary}>
        <View style={s.summaryCard}>
          <Text style={s.summaryLabel}>Przychody</Text>
          <Text style={[s.summaryValue, { color: colors.income }]}>
            +{formatCurrency(totalIncome)}
          </Text>
        </View>
        <View style={s.summaryCard}>
          <Text style={s.summaryLabel}>Wydatki</Text>
          <Text style={[s.summaryValue, { color: colors.expense }]}>
            -{formatCurrency(totalExpense)}
          </Text>
        </View>
      </View>

      <View style={s.filterRow}>
        {(["all", "expense", "income"] as TransactionFilter[]).map((f) => (
          <TouchableOpacity
            key={f}
            style={[s.filterBtn, filter === f && s.filterBtnActive]}
            onPress={() => setFilter(f)}
          >
            <Text style={[s.filterText, filter === f && s.filterTextActive]}>
              {f === "all"
                ? "Wszystkie"
                : f === "expense"
                ? "Wydatki"
                : "Przychody"}
            </Text>
          </TouchableOpacity>
        ))}
      </View>

      <FlatList
        data={filtered}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={s.listContent}
        refreshControl={
          <RefreshControl
            refreshing={refreshing}
            onRefresh={() => {
              setRefreshing(true);
              fetchData();
            }}
          />
        }
        ListEmptyComponent={<Text style={s.emptyText}>Brak transakcji</Text>}
        renderItem={({ item }) => (
          <View style={s.card}>
            <View
              style={[
                s.iconBox,
                { backgroundColor: item.categoryColor + "20" },
              ]}
            >
              <Text style={[s.categoryIconText, { color: item.categoryColor }]}>
                {item.categoryName[0]}
              </Text>
            </View>
            <View style={s.cardInfo}>
              <Text style={s.cardTitle}>{item.title}</Text>
              <Text style={s.cardMeta}>
                {item.categoryName} · {item.accountName} ·{" "}
                {formatDate(item.date)}
              </Text>
              <Text style={s.cardSubMeta}>{item.paymentMethod}</Text>
            </View>
            <View style={s.cardRight}>
              <Text
                style={[
                  s.amount,
                  {
                    color:
                      item.type === "Income" ? colors.income : colors.expense,
                  },
                ]}
              >
                {item.type === "Income" ? "+" : "-"}
                {formatCurrency(item.amount)}
              </Text>
              <View style={s.actions}>
                <TouchableOpacity
                  onPress={() => openEdit(item)}
                  style={s.actionBtn}
                >
                  <MaterialIcons name="edit" size={17} color={colors.primary} />
                </TouchableOpacity>
                <TouchableOpacity
                  onPress={() => handleDelete(item.id, item.title)}
                  style={s.actionBtn}
                >
                  <MaterialIcons
                    name="delete-outline"
                    size={17}
                    color={colors.textMuted}
                  />
                </TouchableOpacity>
              </View>
            </View>
          </View>
        )}
      />

      <TouchableOpacity style={s.fab} onPress={openAdd}>
        <MaterialIcons name="add" size={28} color="#fff" />
      </TouchableOpacity>

      <FormModal
        visible={modalVisible}
        onClose={() => setModalVisible(false)}
        onSave={() => {
          setModalVisible(false);
          fetchData();
        }}
        editing={editing}
        categories={categories}
        accounts={accounts}
      />
    </View>
  );
}
