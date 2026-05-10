export interface ShoppingListDto {
  id: number;
  name: string;
  isCompleted: boolean;
  itemCount: number;
  checkedCount: number;
  createdByName: string;
  createdAt: string;
}

export interface ShoppingItemDto {
  id: number;
  name: string;
  quantity: number;
  estimatedPrice: number | null;
  isChecked: boolean;
  shoppingListId: number;
}

export interface CreateShoppingItemDto {
  name: string;
  quantity: number;
  estimatedPrice?: number | null;
  shoppingListId: number;
}

export interface UpdateShoppingItemDto {
  id: number;
  name: string;
  quantity: number;
  estimatedPrice?: number | null;
  isChecked: boolean;
}
