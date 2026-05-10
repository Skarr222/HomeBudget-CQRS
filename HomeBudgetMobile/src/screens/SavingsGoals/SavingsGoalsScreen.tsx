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
import { savingsGoalsApi } from "../../api/apiService";
import { SavingsGoalDto } from "../../models/types";
import { colors, formatCurrency, formatShortDate } from "../../utils/helpers";
import { s } from "../../styles/SavingsGoals";
import FormModal from "./components/FormModal";

const HOUSEHOLD_ID = 1;

export default function SavingsGoalsScreen() {
  const [goals, setGoals] = useState<SavingsGoalDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editing, setEditing] = useState<SavingsGoalDto | null>(null);

  const fetchData = async () => {
    try {
      const response = await savingsGoalsApi.getAll(HOUSEHOLD_ID);
      setGoals(response.data);
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
    Alert.alert("Usuń cel", `Usunąć "${name}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await savingsGoalsApi.delete(id);
            setGoals((prev) => prev.filter((g) => g.id !== id));
          } catch {
            Alert.alert("Błąd", "Nie udało się usunąć");
          }
        },
      },
    ]);
  };

  const totalSaved = goals.reduce((sum, g) => sum + g.currentAmount, 0);
  const totalTarget = goals.reduce((sum, g) => sum + g.targetAmount, 0);

  if (loading)
    return (
      <View style={s.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );

  return (
    <View style={s.container}>
      <View style={s.hero}>
        <Text style={s.heroLabel}>Łącznie zebrano</Text>
        <Text style={s.heroAmount}>{formatCurrency(totalSaved)}</Text>
        <Text style={s.heroSub}>
          z {formatCurrency(totalTarget)} łącznego celu
        </Text>
      </View>

      <FlatList
        data={goals}
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
            <MaterialIcons name="savings" size={48} color={colors.textMuted} />
            <Text style={s.emptyText}>Brak celów oszczędnościowych</Text>
            <Text style={s.emptySubText}>Dotknij + aby dodać cel</Text>
          </View>
        }
        renderItem={({ item }) => (
          <View style={[s.card, item.isCompleted && s.cardCompleted]}>
            <View
              style={[s.iconCircle, { backgroundColor: item.color + "20" }]}
            >
              <MaterialIcons
                name={item.icon as any}
                size={24}
                color={item.color}
              />
            </View>
            <View style={s.cardContent}>
              <View style={s.cardRow}>
                <View style={s.cardTitleRow}>
                  <Text style={s.cardTitle}>{item.name}</Text>
                  {item.isCompleted && (
                    <MaterialIcons
                      name="check-circle"
                      size={16}
                      color={colors.success}
                    />
                  )}
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
              <View style={s.amountRow}>
                <Text style={s.currentAmount}>
                  {formatCurrency(item.currentAmount)}
                </Text>
                <Text style={s.targetAmount}>
                  / {formatCurrency(item.targetAmount)}
                </Text>
                <Text style={[s.pct, { color: item.color }]}>
                  {item.percentage.toFixed(0)}%
                </Text>
              </View>
              <View style={s.progressBg}>
                <View
                  style={[
                    s.progressFill,
                    {
                      width: `${Math.min(item.percentage, 100)}%` as any,
                      backgroundColor: item.color,
                    },
                  ]}
                />
              </View>
              {item.deadline && (
                <Text style={s.deadline}>
                  Termin: {formatShortDate(item.deadline)}
                </Text>
              )}
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
      />
    </View>
  );
}
