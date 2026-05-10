import React, { useState } from "react";
import {
  View,
  Text,
  Modal,
  TextInput,
  FlatList,
  ActivityIndicator,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  Alert,
} from "react-native";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import { shoppingItemsApi } from "../../../api/apiService";
import { ShoppingItemDto, ShoppingListDto } from "../../../models/types";
import { colors } from "../../../utils/helpers";
import { s, im } from "../../../styles/Shopping";

export default function ItemsModal({
  list,
  visible,
  onClose,
}: {
  list: ShoppingListDto | null;
  visible: boolean;
  onClose: () => void;
}) {
  const [items, setItems] = useState<ShoppingItemDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [addName, setAddName] = useState("");
  const [addQty, setAddQty] = useState("1");
  const [addPrice, setAddPrice] = useState("");
  const [adding, setAdding] = useState(false);
  const [showAddForm, setShowAddForm] = useState(false);

  const fetchItems = React.useCallback(async () => {
    if (!list) return;
    setLoading(true);
    try {
      const response = await shoppingItemsApi.getAll(list.id);
      setItems(response.data);
    } catch {
      Alert.alert("Błąd", "Nie udało się załadować pozycji");
    } finally {
      setLoading(false);
    }
  }, [list]);

  React.useEffect(() => {
    if (visible && list) {
      fetchItems();
      setShowAddForm(false);
      setAddName("");
      setAddQty("1");
      setAddPrice("");
    }
  }, [visible, list, fetchItems]);

  const handleToggle = async (item: ShoppingItemDto) => {
    try {
      await shoppingItemsApi.update(item.id, {
        id: item.id,
        name: item.name,
        quantity: item.quantity,
        estimatedPrice: item.estimatedPrice,
        isChecked: !item.isChecked,
      });
      setItems((prev) =>
        prev.map((i) =>
          i.id === item.id ? { ...i, isChecked: !i.isChecked } : i
        )
      );
    } catch {
      Alert.alert("Błąd", "Nie udało się zaktualizować pozycji");
    }
  };

  const handleAdd = async () => {
    if (!addName.trim() || !list) return;
    setAdding(true);
    try {
      const qty = parseInt(addQty, 10) || 1;
      const price = addPrice ? parseFloat(addPrice) : null;
      const response = await shoppingItemsApi.create({
        name: addName.trim(),
        quantity: qty,
        estimatedPrice: price,
        shoppingListId: list.id,
      });
      const newItem: ShoppingItemDto = {
        id: response.data,
        name: addName.trim(),
        quantity: qty,
        estimatedPrice: price,
        isChecked: false,
        shoppingListId: list.id,
      };
      setItems((prev) => [newItem, ...prev]);
      setAddName("");
      setAddQty("1");
      setAddPrice("");
      setShowAddForm(false);
    } catch {
      Alert.alert("Błąd", "Nie udało się dodać pozycji");
    } finally {
      setAdding(false);
    }
  };

  const handleDelete = (item: ShoppingItemDto) => {
    Alert.alert("Usuń pozycję", `Usunąć "${item.name}"?`, [
      { text: "Anuluj", style: "cancel" },
      {
        text: "Usuń",
        style: "destructive",
        onPress: async () => {
          try {
            await shoppingItemsApi.delete(item.id);
            setItems((prev) => prev.filter((i) => i.id !== item.id));
          } catch {
            Alert.alert("Błąd", "Nie udało się usunąć pozycji");
          }
        },
      },
    ]);
  };

  const checked = items.filter((i) => i.isChecked).length;
  const total = items.length;

  return (
    <Modal
      visible={visible}
      animationType="slide"
      presentationStyle="pageSheet"
      onRequestClose={onClose}
    >
      <View style={im.modalContainer}>
        <View style={im.header}>
          <TouchableOpacity onPress={onClose} style={im.backBtn}>
            <MaterialIcons name="arrow-back" size={22} color={colors.primary} />
          </TouchableOpacity>
          <View style={im.flex1}>
            <Text style={im.headerTitle} numberOfLines={1}>
              {list?.name ?? "Lista zakupów"}
            </Text>
            {total > 0 && (
              <Text style={im.headerSub}>
                {checked}/{total} zaznaczonych
              </Text>
            )}
          </View>
          <TouchableOpacity
            onPress={() => setShowAddForm((v) => !v)}
            style={im.addBtn}
          >
            <MaterialIcons
              name={showAddForm ? "close" : "add"}
              size={22}
              color="#fff"
            />
          </TouchableOpacity>
        </View>

        {total > 0 && (
          <View style={im.progressRow}>
            <View style={im.progressBg}>
              <View
                style={[
                  im.progressFill,
                  {
                    width: `${total > 0 ? (checked / total) * 100 : 0}%` as any,
                  },
                ]}
              />
            </View>
          </View>
        )}

        {showAddForm && (
          <KeyboardAvoidingView
            behavior={Platform.OS === "ios" ? "padding" : undefined}
          >
            <View style={im.addForm}>
              <Text style={im.addFormTitle}>Nowa pozycja</Text>
              <TextInput
                style={im.addInput}
                value={addName}
                onChangeText={setAddName}
                placeholder="Nazwa produktu *"
                placeholderTextColor={colors.textMuted}
                autoFocus
              />
              <View style={im.addRow}>
                <View style={im.flex1}>
                  <Text style={im.addLabel}>Ilość</Text>
                  <TextInput
                    style={im.addInput}
                    value={addQty}
                    onChangeText={setAddQty}
                    keyboardType="numeric"
                    placeholder="1"
                    placeholderTextColor={colors.textMuted}
                  />
                </View>
                <View style={im.flex1}>
                  <Text style={im.addLabel}>Cena (opcja)</Text>
                  <TextInput
                    style={im.addInput}
                    value={addPrice}
                    onChangeText={setAddPrice}
                    keyboardType="decimal-pad"
                    placeholder="0.00"
                    placeholderTextColor={colors.textMuted}
                  />
                </View>
              </View>
              <TouchableOpacity
                style={[
                  im.addConfirmBtn,
                  (!addName.trim() || adding) && im.addConfirmBtnDisabled,
                ]}
                onPress={handleAdd}
                disabled={!addName.trim() || adding}
              >
                <MaterialIcons name="add" size={18} color="#fff" />
                <Text style={im.addConfirmText}>
                  {adding ? "Dodawanie..." : "Dodaj"}
                </Text>
              </TouchableOpacity>
            </View>
          </KeyboardAvoidingView>
        )}

        {loading ? (
          <View style={s.center}>
            <ActivityIndicator size="large" color={colors.primary} />
          </View>
        ) : (
          <FlatList
            data={items.sort(
              (a, b) => Number(a.isChecked) - Number(b.isChecked)
            )}
            keyExtractor={(item) => item.id.toString()}
            contentContainerStyle={im.listContent}
            ListEmptyComponent={
              <View style={s.empty}>
                <MaterialIcons
                  name="playlist-add"
                  size={48}
                  color={colors.textMuted}
                />
                <Text style={s.emptyText}>Brak pozycji</Text>
                <Text style={s.emptySubText}>Dotknij + aby dodać produkt</Text>
              </View>
            }
            renderItem={({ item }) => (
              <View style={[im.itemRow, item.isChecked && im.itemRowDone]}>
                <TouchableOpacity
                  onPress={() => handleToggle(item)}
                  style={im.checkbox}
                >
                  <MaterialIcons
                    name={
                      item.isChecked ? "check-circle" : "radio-button-unchecked"
                    }
                    size={24}
                    color={item.isChecked ? colors.success : colors.textMuted}
                  />
                </TouchableOpacity>
                <View style={im.itemContent}>
                  <Text
                    style={[im.itemName, item.isChecked && im.itemNameDone]}
                  >
                    {item.name}
                  </Text>
                  <Text style={im.itemMeta}>
                    Ilość: {item.quantity}
                    {item.estimatedPrice != null
                      ? `  ·  ${item.estimatedPrice.toFixed(2)} zł`
                      : ""}
                  </Text>
                </View>
                <TouchableOpacity
                  onPress={() => handleDelete(item)}
                  style={im.deleteBtn}
                >
                  <MaterialIcons
                    name="delete-outline"
                    size={20}
                    color={colors.textMuted}
                  />
                </TouchableOpacity>
              </View>
            )}
          />
        )}
      </View>
    </Modal>
  );
}
