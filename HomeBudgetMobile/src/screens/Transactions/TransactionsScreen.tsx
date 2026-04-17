import React, { useState, useCallback } from "react";
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  TouchableOpacity,
  RefreshControl,
  ActivityIndicator,
  Alert,
} from "react-native";
import { useFocusEffect } from "@react-navigation/native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { transactionsApi } from "../../api/apiService";
import { TransactionDto } from "../../models/types";
import { colors, formatCurrency, formatDate } from "../../utils/helpers";

const HOUSEHOLD_ID = 1;

type Filter = "all" | "expense" | "income";

export default function TransactionsScreen() {
  const [transactions, setTransactions] = useState<TransactionDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<Filter>("all");

  const fetchData = async () => {
    try {
      const res = await transactionsApi.getAll({ householdId: HOUSEHOLD_ID });
      setTransactions(res.data);
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
    Alert.alert("Usuń", `Usunąć "${title}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await transactionsApi.delete(id);
            setTransactions((prev) => prev.filter((t) => t.id !== id));
          } catch (err) {
            console.error(err);
            Alert.alert("Błąd", "Nie udało się usunąć");
          }
        },
      },
    ]);
  };

  const filtered = transactions.filter((t) => {
    if (filter === "expense") return t.type === "Expense";
    if (filter === "income") return t.type === "Income";
    return true;
  });

  const totalIncome = transactions
    .filter((t) => t.type === "Income")
    .reduce((s, t) => s + t.amount, 0);
  const totalExpense = transactions
    .filter((t) => t.type === "Expense")
    .reduce((s, t) => s + t.amount, 0);

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {/* Summary */}
      <View style={styles.summary}>
        <View style={styles.summaryCard}>
          <Text style={styles.summaryLabel}>Przychody</Text>
          <Text style={[styles.summaryValue, { color: colors.income }]}>
            +{formatCurrency(totalIncome)}
          </Text>
        </View>
        <View style={styles.summaryCard}>
          <Text style={styles.summaryLabel}>Wydatki</Text>
          <Text style={[styles.summaryValue, { color: colors.expense }]}>
            -{formatCurrency(totalExpense)}
          </Text>
        </View>
      </View>

      {/* Filter tabs */}
      <View style={styles.filterRow}>
        {(["all", "expense", "income"] as Filter[]).map((f) => (
          <TouchableOpacity
            key={f}
            style={[styles.filterBtn, filter === f && styles.filterBtnActive]}
            onPress={() => setFilter(f)}
          >
            <Text
              style={[
                styles.filterText,
                filter === f && styles.filterTextActive,
              ]}
            >
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
        renderItem={({ item }) => (
          <View style={styles.card}>
            <View
              style={[
                styles.iconBox,
                { backgroundColor: item.categoryColor + "20" },
              ]}
            >
              <Text style={{ color: item.categoryColor, fontWeight: "700" }}>
                {item.categoryName[0]}
              </Text>
            </View>
            <View style={styles.cardInfo}>
              <Text style={styles.cardTitle}>{item.title}</Text>
              <Text style={styles.cardMeta}>
                {item.categoryName} · {item.accountName} ·{" "}
                {formatDate(item.date)}
              </Text>
              <Text style={styles.cardSubMeta}>
                {item.userName} · {item.paymentMethod}
              </Text>
              {(item.tags.length > 0 ||
                item.isShared ||
                item.hasReceipt ||
                item.hasSplit) && (
                <View style={styles.badgesRow}>
                  {item.tags.map((tag, i) => (
                    <View key={i} style={styles.tag}>
                      <Text style={styles.tagText}>{tag}</Text>
                    </View>
                  ))}
                  {item.isShared && (
                    <View style={styles.badgeShared}>
                      <MaterialIcons
                        name="people"
                        size={11}
                        color={colors.primary}
                      />
                      <Text style={styles.badgeSharedText}>Wspólny</Text>
                    </View>
                  )}
                  {item.hasReceipt && (
                    <View style={styles.badgeReceipt}>
                      <MaterialIcons
                        name="receipt-long"
                        size={11}
                        color={colors.warning}
                      />
                    </View>
                  )}
                  {item.hasSplit && (
                    <View style={styles.badgeSplit}>
                      <MaterialIcons
                        name="call-split"
                        size={11}
                        color={colors.success}
                      />
                    </View>
                  )}
                </View>
              )}
            </View>
            <View style={styles.cardRight}>
              <Text
                style={[
                  styles.amount,
                  {
                    color:
                      item.type === "Income" ? colors.income : colors.expense,
                  },
                ]}
              >
                {item.type === "Income" ? "+" : "-"}
                {formatCurrency(item.amount)}
              </Text>
              <TouchableOpacity
                onPress={() => handleDelete(item.id, item.title)}
                style={styles.deleteBtn}
              >
                <MaterialIcons
                  name="delete-outline"
                  size={18}
                  color={colors.textMuted}
                />
              </TouchableOpacity>
            </View>
          </View>
        )}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={{ padding: 16, paddingBottom: 80 }}
        refreshControl={
          <RefreshControl
            refreshing={refreshing}
            onRefresh={() => {
              setRefreshing(true);
              fetchData();
            }}
          />
        }
        ListEmptyComponent={
          <Text style={styles.emptyText}>Brak transakcji</Text>
        }
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background },
  center: { flex: 1, justifyContent: "center", alignItems: "center" },
  emptyText: {
    textAlign: "center",
    color: colors.textMuted,
    marginTop: 40,
    fontSize: 16,
  },

  summary: {
    flexDirection: "row",
    paddingHorizontal: 16,
    paddingTop: 16,
    gap: 10,
  },
  summaryCard: {
    flex: 1,
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 14,
  },
  summaryLabel: { fontSize: 12, color: colors.textMuted },
  summaryValue: { fontSize: 18, fontWeight: "700", marginTop: 4 },

  filterRow: { flexDirection: "row", padding: 16, gap: 8 },
  filterBtn: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 20,
    backgroundColor: colors.surface,
    borderWidth: 1,
    borderColor: colors.border,
  },
  filterBtnActive: {
    backgroundColor: colors.primary,
    borderColor: colors.primary,
  },
  filterText: { fontSize: 13, color: colors.textSecondary, fontWeight: "600" },
  filterTextActive: { color: "#fff" },

  card: {
    flexDirection: "row",
    backgroundColor: colors.surface,
    borderRadius: 14,
    padding: 14,
    marginBottom: 10,
  },
  iconBox: {
    width: 42,
    height: 42,
    borderRadius: 12,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  cardInfo: { flex: 1 },
  cardTitle: { color: colors.text, fontWeight: "600", fontSize: 14 },
  cardMeta: { color: colors.textMuted, fontSize: 11, marginTop: 2 },
  cardSubMeta: { color: colors.textMuted, fontSize: 10, marginTop: 1 },
  badgesRow: {
    flexDirection: "row",
    flexWrap: "wrap",
    marginTop: 6,
    gap: 4,
    alignItems: "center",
  },
  tag: {
    backgroundColor: colors.surfaceSecondary,
    borderRadius: 6,
    paddingHorizontal: 6,
    paddingVertical: 2,
  },
  tagText: { fontSize: 10, color: colors.textSecondary },
  badgeShared: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: colors.primary + "15",
    borderRadius: 6,
    paddingHorizontal: 6,
    paddingVertical: 2,
    gap: 3,
  },
  badgeSharedText: { fontSize: 10, color: colors.primary, fontWeight: "600" },
  badgeReceipt: {
    backgroundColor: colors.warning + "15",
    borderRadius: 6,
    padding: 3,
  },
  badgeSplit: {
    backgroundColor: colors.success + "15",
    borderRadius: 6,
    padding: 3,
  },
  cardRight: { alignItems: "flex-end", justifyContent: "space-between" },
  amount: { fontWeight: "700", fontSize: 15 },
  deleteBtn: { marginTop: 4 },
});
