import React, { useState, useCallback } from "react";
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  RefreshControl,
  ActivityIndicator,
} from "react-native";
import { useFocusEffect } from "@react-navigation/native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { dashboardApi } from "../../api/apiService";
import { DashboardDto } from "../../models/types";
import {
  colors,
  formatCurrency,
  formatShortDate,
  getMonthName,
} from "../../utils/helpers";

const HOUSEHOLD_ID = 1;

export default function DashboardScreen() {
  const [data, setData] = useState<DashboardDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const fetchData = async () => {
    try {
      const res = await dashboardApi.get(HOUSEHOLD_ID);
      setData(res.data);
    } catch (err) {
      console.error("Dashboard error:", err);
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

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );
  }

  if (!data) {
    return (
      <View style={styles.center}>
        <Text style={styles.errorText}>Nie udało się załadować danych</Text>
        <Text style={styles.errorSubtext}>
          Sprawdź czy API działa na http://10.0.2.2:5067
        </Text>
      </View>
    );
  }

  const now = new Date();
  const monthLabel = `${getMonthName(now.getMonth() + 1)} ${now.getFullYear()}`;

  return (
    <ScrollView
      style={styles.container}
      refreshControl={
        <RefreshControl
          refreshing={refreshing}
          onRefresh={() => {
            setRefreshing(true);
            fetchData();
          }}
        />
      }
    >
      {/* Hero balance */}
      <View style={styles.hero}>
        <Text style={styles.heroLabel}>Bilans · {monthLabel}</Text>
        <Text style={styles.heroAmount}>
          {data.balance >= 0 ? "+" : ""}
          {formatCurrency(data.balance)}
        </Text>
        <View style={styles.heroRow}>
          <View style={styles.heroItem}>
            <MaterialIcons
              name="trending-up"
              size={16}
              color={colors.successLight}
            />
            <Text style={styles.heroItemLabel}>Przychody</Text>
            <Text
              style={[styles.heroItemValue, { color: colors.successLight }]}
            >
              {formatCurrency(data.totalIncome)}
            </Text>
          </View>
          <View style={styles.heroDivider} />
          <View style={styles.heroItem}>
            <MaterialIcons
              name="trending-down"
              size={16}
              color={colors.dangerLight}
            />
            <Text style={styles.heroItemLabel}>Wydatki</Text>
            <Text style={[styles.heroItemValue, { color: colors.dangerLight }]}>
              {formatCurrency(data.totalExpenses)}
            </Text>
          </View>
        </View>
      </View>

      {/* Accounts strip */}
      {data.accounts.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Konta</Text>
          <ScrollView
            horizontal
            showsHorizontalScrollIndicator={false}
            contentContainerStyle={{ gap: 10 }}
          >
            {data.accounts.map((a) => (
              <View
                key={a.id}
                style={[
                  styles.accountCard,
                  { borderLeftColor: a.color, borderLeftWidth: 4 },
                ]}
              >
                <Text style={styles.accountName}>{a.name}</Text>
                <Text style={styles.accountUser}>{a.userName}</Text>
                <Text
                  style={[
                    styles.accountBalance,
                    { color: a.balance >= 0 ? colors.text : colors.danger },
                  ]}
                >
                  {formatCurrency(a.balance)}
                </Text>
              </View>
            ))}
          </ScrollView>
        </View>
      )}

      {/* Top categories */}
      {data.topCategories.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Top kategorie wydatków</Text>
          {data.topCategories.map((cat, i) => (
            <View key={i} style={styles.catRow}>
              <View style={[styles.catDot, { backgroundColor: cat.color }]} />
              <View style={styles.catInfo}>
                <Text style={styles.catName}>{cat.name}</Text>
                <Text style={styles.catSubtext}>
                  {cat.transactionCount} transakcji
                </Text>
              </View>
              <View style={styles.catRight}>
                <Text style={styles.catAmount}>
                  {formatCurrency(cat.amount)}
                </Text>
                <Text style={styles.catPct}>{cat.percentage.toFixed(0)}%</Text>
              </View>
            </View>
          ))}
        </View>
      )}

      {/* Upcoming bills */}
      {data.upcomingBills.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Nadchodzące rachunki</Text>
          {data.upcomingBills.map((b) => (
            <View key={b.id} style={styles.billRow}>
              <View style={styles.billInfo}>
                <Text style={styles.billName}>{b.billName}</Text>
                <Text style={styles.billDate}>
                  Termin: {formatShortDate(b.dueDate)}
                </Text>
              </View>
              <Text style={styles.billAmount}>{formatCurrency(b.amount)}</Text>
            </View>
          ))}
        </View>
      )}

      {/* Budgets */}
      {data.activeBudgets.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Budżety</Text>
          {data.activeBudgets.map((budget) => (
            <View key={budget.id} style={styles.budgetCard}>
              <View style={styles.budgetHeader}>
                <Text style={styles.budgetName}>{budget.categoryName}</Text>
                <Text style={styles.budgetAmount}>
                  {formatCurrency(budget.spent)} /{" "}
                  {formatCurrency(budget.amount)}
                </Text>
              </View>
              <View style={styles.progressBg}>
                <View
                  style={[
                    styles.progressFill,
                    {
                      width: `${Math.min(budget.percentage, 100)}%`,
                      backgroundColor:
                        budget.percentage > 90
                          ? colors.danger
                          : budget.percentage > 70
                          ? colors.warning
                          : colors.success,
                    },
                  ]}
                />
              </View>
            </View>
          ))}
        </View>
      )}

      {/* Goals */}
      {data.savingsGoals.length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Cele oszczędnościowe</Text>
          {data.savingsGoals.map((g) => (
            <View key={g.id} style={styles.goalCard}>
              <Text style={styles.goalName}>{g.name}</Text>
              <Text style={styles.goalAmount}>
                {formatCurrency(g.currentAmount)} /{" "}
                {formatCurrency(g.targetAmount)}
              </Text>
              <View style={styles.progressBg}>
                <View
                  style={[
                    styles.progressFill,
                    {
                      width: `${Math.min(g.percentage, 100)}%`,
                      backgroundColor: g.color,
                    },
                  ]}
                />
              </View>
            </View>
          ))}
        </View>
      )}

      {/* Recent */}
      <View style={[styles.section, { marginBottom: 32 }]}>
        <Text style={styles.sectionTitle}>Ostatnie transakcje</Text>
        {data.recentTransactions.slice(0, 5).map((t) => (
          <View key={t.id} style={styles.txRow}>
            <View
              style={[
                styles.txIcon,
                { backgroundColor: t.categoryColor + "20" },
              ]}
            >
              <Text
                style={{
                  color: t.categoryColor,
                  fontWeight: "700",
                  fontSize: 12,
                }}
              >
                {t.categoryName[0]}
              </Text>
            </View>
            <View style={styles.txInfo}>
              <Text style={styles.txTitle}>{t.title}</Text>
              <Text style={styles.txMeta}>
                {t.userName} · {formatShortDate(t.date)}
              </Text>
            </View>
            <Text
              style={[
                styles.txAmount,
                { color: t.type === "Income" ? colors.income : colors.expense },
              ]}
            >
              {t.type === "Income" ? "+" : "-"}
              {formatCurrency(t.amount)}
            </Text>
          </View>
        ))}
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background },
  center: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    padding: 20,
  },
  errorText: { color: colors.text, fontSize: 16, fontWeight: "600" },
  errorSubtext: {
    color: colors.textMuted,
    fontSize: 13,
    marginTop: 6,
    textAlign: "center",
  },

  hero: {
    backgroundColor: colors.primary,
    margin: 16,
    borderRadius: 20,
    padding: 24,
  },
  heroLabel: { color: "rgba(255,255,255,0.7)", fontSize: 13 },
  heroAmount: { color: "#fff", fontSize: 36, fontWeight: "800", marginTop: 4 },
  heroRow: { flexDirection: "row", marginTop: 20 },
  heroItem: { flex: 1 },
  heroDivider: { width: 1, backgroundColor: "rgba(255,255,255,0.15)" },
  heroItemLabel: { color: "rgba(255,255,255,0.6)", fontSize: 11, marginTop: 4 },
  heroItemValue: { fontSize: 16, fontWeight: "700", marginTop: 2 },

  section: {
    backgroundColor: colors.surface,
    marginHorizontal: 16,
    marginTop: 12,
    borderRadius: 16,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: "700",
    color: colors.text,
    marginBottom: 12,
  },

  accountCard: {
    backgroundColor: colors.surfaceSecondary,
    borderRadius: 12,
    padding: 12,
    minWidth: 160,
  },
  accountName: { fontSize: 14, fontWeight: "700", color: colors.text },
  accountUser: { fontSize: 11, color: colors.textMuted, marginTop: 2 },
  accountBalance: { fontSize: 16, fontWeight: "700", marginTop: 8 },

  catRow: { flexDirection: "row", alignItems: "center", paddingVertical: 8 },
  catDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  catInfo: { flex: 1 },
  catName: { color: colors.text, fontSize: 14, fontWeight: "500" },
  catSubtext: { color: colors.textMuted, fontSize: 11, marginTop: 1 },
  catRight: { alignItems: "flex-end" },
  catAmount: { color: colors.text, fontWeight: "600", fontSize: 14 },
  catPct: { color: colors.textMuted, fontSize: 11 },

  billRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingVertical: 8,
  },
  billInfo: { flex: 1 },
  billName: { color: colors.text, fontWeight: "600", fontSize: 14 },
  billDate: { color: colors.textMuted, fontSize: 12, marginTop: 2 },
  billAmount: { color: colors.warning, fontWeight: "700", fontSize: 14 },

  budgetCard: { marginBottom: 12 },
  budgetHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 6,
  },
  budgetName: { color: colors.text, fontWeight: "600", fontSize: 14 },
  budgetAmount: { color: colors.textSecondary, fontSize: 12 },
  progressBg: {
    height: 8,
    backgroundColor: colors.surfaceSecondary,
    borderRadius: 4,
    overflow: "hidden",
  },
  progressFill: { height: "100%", borderRadius: 4 },

  goalCard: { marginBottom: 12 },
  goalName: { color: colors.text, fontWeight: "600", fontSize: 14 },
  goalAmount: { color: colors.textSecondary, fontSize: 12, marginBottom: 6 },

  txRow: {
    flexDirection: "row",
    alignItems: "center",
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
  },
  txIcon: {
    width: 36,
    height: 36,
    borderRadius: 10,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  txInfo: { flex: 1 },
  txTitle: { color: colors.text, fontWeight: "500", fontSize: 14 },
  txMeta: { color: colors.textMuted, fontSize: 12, marginTop: 2 },
  txAmount: { fontWeight: "700", fontSize: 14 },
});
