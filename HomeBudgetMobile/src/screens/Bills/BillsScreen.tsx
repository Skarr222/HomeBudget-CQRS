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
import { accountsApi, billsApi, categoriesApi } from "../../api/apiService";
import { AccountDto, BillDto, CategoryDto } from "../../models/types";
import { colors, formatCurrency } from "../../utils/helpers";
import { s } from "../../styles/Bills";
import FormModal from "./components/FormModal";
import PayModal from "./components/PayModal";

const HOUSEHOLD_ID = 1;

export default function BillsScreen() {
  const [bills, setBills] = useState<BillDto[]>([]);
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editing, setEditing] = useState<BillDto | null>(null);
  const [payModalVisible, setPayModalVisible] = useState(false);
  const [paying, setPaying] = useState<BillDto | null>(null);

  const fetchData = async () => {
    try {
      const [bRes, cRes, aRes] = await Promise.all([
        billsApi.getAll(HOUSEHOLD_ID),
        categoriesApi.getAll(),
        accountsApi.getAll({ householdId: HOUSEHOLD_ID }),
      ]);
      setBills(bRes.data);
      setCategories(cRes.data);
      setAccounts(aRes.data);
    } catch (e) {
      console.error(e);
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

  const handleDelete = (id: number, name: string) => {
    Alert.alert("Usuń rachunek", `Usunąć "${name}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await billsApi.delete(id);
            setBills((prev) => prev.filter((b) => b.id !== id));
          } catch {
            Alert.alert("Błąd", "Nie udało się usunąć");
          }
        },
      },
    ]);
  };

  const totalMonthly = bills
    .filter((b) => b.isActive)
    .reduce((sum, b) => sum + b.estimatedAmount, 0);

  if (loading)
    return (
      <View style={s.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );

  return (
    <View style={s.container}>
      <View style={s.hero}>
        <Text style={s.heroLabel}>Miesięczne zobowiązania</Text>
        <Text style={s.heroAmount}>{formatCurrency(totalMonthly)}</Text>
        <Text style={s.heroSub}>
          {bills.filter((b) => b.isActive).length} aktywnych rachunków
        </Text>
      </View>

      <FlatList
        data={bills}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={{ padding: 16, paddingBottom: 100 }}
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
          <View style={s.empty}>
            <MaterialIcons
              name="receipt-long"
              size={48}
              color={colors.textMuted}
            />
            <Text style={s.emptyText}>Brak rachunków</Text>
            <Text style={s.emptySubText}>Dotknij + aby dodać rachunek</Text>
          </View>
        }
        renderItem={({ item }) => (
          <View style={[s.card, !item.isActive && s.cardInactive]}>
            <View
              style={[s.iconCircle, { backgroundColor: item.color + "20" }]}
            >
              <MaterialIcons
                name={item.icon as any}
                size={22}
                color={item.color}
              />
            </View>
            <View style={s.cardContent}>
              <View style={s.cardRow}>
                <View>
                  <Text style={s.cardTitle}>{item.name}</Text>
                  {item.provider ? (
                    <Text style={s.provider}>{item.provider}</Text>
                  ) : null}
                </View>
                <View style={s.cardActions}>
                  {item.isActive && !item.paidThisMonth && (
                    <TouchableOpacity
                      onPress={() => {
                        setPaying(item);
                        setPayModalVisible(true);
                      }}
                      style={s.actionBtn}
                    >
                      <MaterialIcons
                        name="check-circle-outline"
                        size={17}
                        color={colors.success}
                      />
                    </TouchableOpacity>
                  )}
                  <TouchableOpacity
                    onPress={() => {
                      setEditing(item);
                      setModalVisible(true);
                    }}
                    style={s.actionBtn}
                  >
                    <MaterialIcons
                      name="edit"
                      size={17}
                      color={colors.primary}
                    />
                  </TouchableOpacity>
                  <TouchableOpacity
                    onPress={() => handleDelete(item.id, item.name)}
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
              <View style={s.metaRow}>
                <View style={s.badge}>
                  <MaterialIcons
                    name="event"
                    size={12}
                    color={colors.textMuted}
                  />
                  <Text style={s.badgeText}>
                    {item.dueDay}. każdego miesiąca
                  </Text>
                </View>
                <Text style={s.amount}>
                  {formatCurrency(item.estimatedAmount)}
                </Text>
              </View>
              <Text style={s.category}>{item.categoryName}</Text>
              {item.paidThisMonth && (
                <View style={s.paidBadge}>
                  <MaterialIcons name="check-circle" size={12} color={colors.success} />
                  <Text style={s.paidBadgeText}>Opłacony w tym miesiącu</Text>
                </View>
              )}
              {!item.isActive && <Text style={s.inactive}>Nieaktywny</Text>}
            </View>
          </View>
        )}
      />

      <TouchableOpacity
        style={s.fab}
        onPress={() => {
          setEditing(null);
          setModalVisible(true);
        }}
      >
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
      />

      <PayModal
        visible={payModalVisible}
        onClose={() => setPayModalVisible(false)}
        onPaid={() => {
          setPayModalVisible(false);
          fetchData();
        }}
        bill={paying}
        accounts={accounts}
      />
    </View>
  );
}
