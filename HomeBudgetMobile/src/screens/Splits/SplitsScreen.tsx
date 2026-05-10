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
import { accountsApi } from "../../api/apiService";
import { AccountDto } from "../../models/types";
import { colors, formatCurrency } from "../../utils/helpers";
import { s } from "../../styles/Accounts";
import { ACCOUNT_TYPES } from "../../utils/Accounts/constants";
import FormModal from "./components/FormModal";

const HOUSEHOLD_ID = 1;

export default function SplitsScreen() {
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editing, setEditing] = useState<AccountDto | null>(null);

  const fetchData = async () => {
    try {
      const response = await accountsApi.getAll({ householdId: HOUSEHOLD_ID });
      setAccounts(response.data);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useFocusEffect(
    useCallback(() => {
      fetchData();
    }, [])
  );

  const handleDelete = (id: number, name: string) => {
    Alert.alert(
      "Usuń konto",
      `Usunąć konto "${name}"?\nTransakcje powiązane z tym kontem nie zostaną usunięte.`,
      [
        { text: "Anuluj", style: "cancel" },
        {
          text: "Usuń",
          style: "destructive",
          onPress: async () => {
            try {
              await accountsApi.delete(id);
              setAccounts((prev) => prev.filter((a) => a.id !== id));
            } catch {
              Alert.alert("Błąd", "Nie udało się usunąć konta");
            }
          },
        },
      ]
    );
  };

  const totalBalance = accounts.reduce((sum, a) => sum + a.balance, 0);

  if (loading)
    return (
      <View style={s.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );

  return (
    <View style={s.container}>
      <View style={s.hero}>
        <Text style={s.heroLabel}>Łączne saldo</Text>
        <Text
          style={[
            s.heroAmount,
            { color: totalBalance >= 0 ? "#fff" : colors.dangerLight },
          ]}
        >
          {formatCurrency(totalBalance)}
        </Text>
        <Text style={s.heroSub}>{accounts.length} kont</Text>
      </View>

      <FlatList
        data={accounts}
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
              name="account-balance"
              size={48}
              color={colors.textMuted}
            />
            <Text style={s.emptyText}>Brak kont</Text>
            <Text style={s.emptySubText}>Dotknij + aby dodać konto</Text>
          </View>
        }
        renderItem={({ item }) => {
          const typeInfo = ACCOUNT_TYPES.find((t) => t.value === item.type);
          return (
            <View
              style={[
                s.card,
                { borderLeftColor: item.color, borderLeftWidth: 4 },
              ]}
            >
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
                    <Text style={s.cardSub}>
                      {typeInfo?.label ?? item.type} · {item.userName}
                    </Text>
                  </View>
                  <View style={s.cardActions}>
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
                <Text
                  style={[
                    s.balance,
                    {
                      color: item.balance >= 0 ? colors.success : colors.danger,
                    },
                  ]}
                >
                  {formatCurrency(item.balance)}
                </Text>
              </View>
            </View>
          );
        }}
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
      />
    </View>
  );
}
