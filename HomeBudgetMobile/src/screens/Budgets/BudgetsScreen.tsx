import React, { useState, useCallback } from "react";
import {
  View, Text, FlatList, TouchableOpacity,
  RefreshControl, ActivityIndicator, Alert,
} from "react-native";
import { useFocusEffect } from "@react-navigation/native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { budgetsApi, categoriesApi } from "../../api/apiService";
import { BudgetDto, CategoryDto } from "../../models/types";
import { colors, formatCurrency, getMonthName } from "../../utils/helpers";
import { s } from "../../styles/Budgets";
import FormModal from "./components/FormModal";

const HOUSEHOLD_ID = 1;
const now = new Date();

export default function BudgetsScreen() {
  const [budgets, setBudgets]           = useState<BudgetDto[]>([]);
  const [categories, setCategories]     = useState<CategoryDto[]>([]);
  const [loading, setLoading]           = useState(true);
  const [refreshing, setRefreshing]     = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editing, setEditing]           = useState<BudgetDto | null>(null);
  const [month] = useState(now.getMonth() + 1);
  const [year]  = useState(now.getFullYear());

  const fetchData = async () => {
    try {
      const [bRes, cRes] = await Promise.all([
        budgetsApi.getAll({ householdId: HOUSEHOLD_ID, month, year }),
        categoriesApi.getAll(),
      ]);
      setBudgets(bRes.data);
      setCategories(cRes.data);
    } catch (e) { console.error(e); }
    finally { setLoading(false); setRefreshing(false); }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useFocusEffect(useCallback(() => { fetchData(); }, []));

  const handleDelete = (id: number, name: string) => {
    Alert.alert("Usuń budżet", `Usunąć budżet dla "${name}"?`, [
      { text: "Anuluj", style: "cancel" },
      { text: "Usuń", style: "destructive", onPress: async () => {
        try { await budgetsApi.delete(id); setBudgets(prev => prev.filter(b => b.id !== id)); }
        catch { Alert.alert("Błąd", "Nie udało się usunąć"); }
      }},
    ]);
  };

  const totalBudget = budgets.reduce((sum, b) => sum + b.amount, 0);
  const totalSpent  = budgets.reduce((sum, b) => sum + b.spent, 0);

  if (loading) return <View style={s.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

  return (
    <View style={s.container}>
      <View style={s.hero}>
        <Text style={s.heroLabel}>{getMonthName(month)} {year}</Text>
        <View style={s.heroRow}>
          <View style={s.heroItem}>
            <Text style={s.heroItemLabel}>Łączny limit</Text>
            <Text style={s.heroItemValue}>{formatCurrency(totalBudget)}</Text>
          </View>
          <View style={s.heroDivider} />
          <View style={s.heroItem}>
            <Text style={s.heroItemLabel}>Wydano</Text>
            <Text style={[s.heroItemValue, { color: totalSpent > totalBudget ? colors.danger : colors.success }]}>
              {formatCurrency(totalSpent)}
            </Text>
          </View>
        </View>
      </View>

      <FlatList
        data={budgets}
        keyExtractor={item => item.id.toString()}
        contentContainerStyle={{ padding: 16, paddingBottom: 100 }}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={() => { setRefreshing(true); fetchData(); }} />}
        ListEmptyComponent={
          <View style={s.empty}>
            <MaterialIcons name="account-balance-wallet" size={48} color={colors.textMuted} />
            <Text style={s.emptyText}>Brak budżetów na ten miesiąc</Text>
            <Text style={s.emptySubText}>Dotknij + aby dodać limit</Text>
          </View>
        }
        renderItem={({ item }) => {
          const pct = Math.min(item.percentage, 100);
          const barColor = item.percentage > 90 ? colors.danger : item.percentage > 70 ? colors.warning : colors.success;
          return (
            <View style={s.card}>
              <View style={[s.catDot, { backgroundColor: item.categoryColor }]} />
              <View style={s.cardContent}>
                <View style={s.cardRow}>
                  <Text style={s.cardTitle}>{item.categoryName}</Text>
                  <View style={s.cardActions}>
                    <TouchableOpacity onPress={() => { setEditing(item); setModalVisible(true); }} style={s.actionBtn}>
                      <MaterialIcons name="edit" size={17} color={colors.primary} />
                    </TouchableOpacity>
                    <TouchableOpacity onPress={() => handleDelete(item.id, item.categoryName)} style={s.actionBtn}>
                      <MaterialIcons name="delete-outline" size={17} color={colors.textMuted} />
                    </TouchableOpacity>
                  </View>
                </View>
                <View style={s.amountRow}>
                  <Text style={s.spent}>{formatCurrency(item.spent)}</Text>
                  <Text style={s.limit}>/ {formatCurrency(item.amount)}</Text>
                  <Text style={[s.pct, { color: barColor }]}>{item.percentage.toFixed(0)}%</Text>
                </View>
                <View style={s.progressBg}>
                  <View style={[s.progressFill, { width: `${pct}%` as any, backgroundColor: barColor }]} />
                </View>
                <Text style={s.remaining}>
                  {item.remaining >= 0
                    ? `Pozostało ${formatCurrency(item.remaining)}`
                    : `Przekroczono o ${formatCurrency(-item.remaining)}`}
                </Text>
              </View>
            </View>
          );
        }}
      />

      <TouchableOpacity style={s.fab} onPress={() => { setEditing(null); setModalVisible(true); }}>
        <MaterialIcons name="add" size={28} color="#fff" />
      </TouchableOpacity>

      <FormModal
        visible={modalVisible} onClose={() => setModalVisible(false)}
        onSave={() => { setModalVisible(false); fetchData(); }}
        editing={editing} categories={categories} month={month} year={year}
      />
    </View>
  );
}
