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
import { shoppingListsApi } from "../../api/apiService";
import { ShoppingListDto } from "../../models/types";
import { colors, formatDate } from "../../utils/helpers";
import { s } from "../../styles/Shopping";
import ListFormModal from "./components/ListFormModal";
import ItemsModal from "./components/ItemsModal";

const HOUSEHOLD_ID = 1;

export default function ShoppingScreen() {
  const [lists, setLists] = useState<ShoppingListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [listFormVisible, setListFormVisible] = useState(false);
  const [editingList, setEditingList] = useState<ShoppingListDto | null>(null);
  const [selectedList, setSelectedList] = useState<ShoppingListDto | null>(null);
  const [itemsModalVisible, setItemsModalVisible] = useState(false);

  const fetchData = async () => {
    try {
      const response = await shoppingListsApi.getAll(HOUSEHOLD_ID);
      setLists(response.data);
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
    Alert.alert("Usuń listę", `Usunąć listę "${name}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await shoppingListsApi.delete(id);
            setLists((prev) => prev.filter((l) => l.id !== id));
          } catch {
            Alert.alert("Błąd", "Nie udało się usunąć listy");
          }
        },
      },
    ]);
  };

  const openItems = (list: ShoppingListDto) => {
    setSelectedList(list);
    setItemsModalVisible(true);
  };

  const active = lists.filter((l) => !l.isCompleted).length;
  const completed = lists.filter((l) => l.isCompleted).length;

  if (loading)
    return (
      <View style={s.center}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );

  return (
    <View style={s.container}>
      <View style={s.statsRow}>
        <View style={s.statCard}>
          <MaterialIcons name="shopping-cart" size={20} color={colors.primary} />
          <Text style={s.statValue}>{active}</Text>
          <Text style={s.statLabel}>Aktywne</Text>
        </View>
        <View style={s.statCard}>
          <MaterialIcons name="check-circle" size={20} color={colors.success} />
          <Text style={s.statValue}>{completed}</Text>
          <Text style={s.statLabel}>Zakończone</Text>
        </View>
      </View>

      <FlatList
        data={lists}
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
            <MaterialIcons name="shopping-cart" size={48} color={colors.textMuted} />
            <Text style={s.emptyText}>Brak list zakupów</Text>
            <Text style={s.emptySubText}>Dotknij + aby utworzyć listę</Text>
          </View>
        }
        renderItem={({ item }) => (
          <TouchableOpacity
            style={[s.card, item.isCompleted && s.cardCompleted]}
            onPress={() => openItems(item)}
            activeOpacity={0.75}
          >
            <View
              style={[
                s.iconCircle,
                {
                  backgroundColor: item.isCompleted
                    ? colors.success + "20"
                    : colors.primary + "15",
                },
              ]}
            >
              <MaterialIcons
                name={item.isCompleted ? "check-circle" : "shopping-cart"}
                size={22}
                color={item.isCompleted ? colors.success : colors.primary}
              />
            </View>
            <View style={s.cardContent}>
              <View style={s.cardRow}>
                <Text style={[s.cardTitle, item.isCompleted && s.cardTitleDone]}>
                  {item.name}
                </Text>
                <View style={s.cardActions}>
                  <TouchableOpacity
                    onPress={() => {
                      setEditingList(item);
                      setListFormVisible(true);
                    }}
                    style={s.actionBtn}
                  >
                    <MaterialIcons name="edit" size={17} color={colors.primary} />
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
                <Text style={s.metaText}>{item.createdByName}</Text>
                <Text style={s.metaDot}>·</Text>
                <Text style={s.metaText}>{formatDate(item.createdAt)}</Text>
                <Text style={s.metaDot}>·</Text>
                <Text style={[s.metaText, { color: colors.primary }]}>
                  Otwórz →
                </Text>
              </View>
              {item.itemCount > 0 && (
                <View style={s.progressRow}>
                  <View style={s.progressBg}>
                    <View
                      style={[
                        s.progressFill,
                        {
                          width: `${
                            (item.checkedCount / item.itemCount) * 100
                          }%` as any,
                          backgroundColor:
                            item.checkedCount === item.itemCount
                              ? colors.success
                              : colors.primary,
                        },
                      ]}
                    />
                  </View>
                  <Text style={s.progressText}>
                    {item.checkedCount}/{item.itemCount}
                  </Text>
                </View>
              )}
              {item.itemCount === 0 && (
                <Text style={s.emptyItemsHint}>
                  Brak pozycji — dotknij aby dodać
                </Text>
              )}
            </View>
          </TouchableOpacity>
        )}
      />

      <TouchableOpacity
        style={s.fab}
        onPress={() => {
          setEditingList(null);
          setListFormVisible(true);
        }}
      >
        <MaterialIcons name="add" size={28} color="#fff" />
      </TouchableOpacity>

      <ListFormModal
        visible={listFormVisible}
        onClose={() => setListFormVisible(false)}
        onSave={() => {
          setListFormVisible(false);
          fetchData();
        }}
        editing={editingList}
      />

      <ItemsModal
        list={selectedList}
        visible={itemsModalVisible}
        onClose={() => {
          setItemsModalVisible(false);
          fetchData();
        }}
      />
    </View>
  );
}
