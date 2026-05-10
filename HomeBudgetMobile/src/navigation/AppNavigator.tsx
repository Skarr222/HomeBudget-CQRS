import React from "react";
import { NavigationContainer } from "@react-navigation/native";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";

import DashboardScreen from "../screens/Dashboard/DashboardScreen";
import TransactionsScreen from "../screens/Transactions/TransactionsScreen";
import BudgetsScreen from "../screens/Budgets/BudgetsScreen";
import SavingsGoalsScreen from "../screens/SavingsGoals/SavingsGoalsScreen";
import BillsScreen from "../screens/Bills/BillsScreen";
import ShoppingScreen from "../screens/Shopping/ShoppingScreen";
import SplitsScreen from "../screens/Splits/SplitsScreen";

import { colors } from "../utils/helpers";

const Tab = createBottomTabNavigator();

const tabIcons: Record<string, string> = {
  Dashboard: "dashboard",
  Transakcje: "swap-horiz",
  Budżety: "account-balance-wallet",
  Cele: "savings",
  Rachunki: "receipt",
  Zakupy: "shopping-cart",
  Konta: "account-balance",
};

export default function AppNavigator() {
  return (
    <NavigationContainer>
      <Tab.Navigator
        screenOptions={({ route }) => ({
          tabBarIcon: ({ color, size }) => (
            <MaterialIcons
              name={tabIcons[route.name] || "home"}
              size={size}
              color={color}
            />
          ),
          tabBarActiveTintColor: colors.primary,
          tabBarInactiveTintColor: colors.textMuted,
          tabBarStyle: {
            backgroundColor: colors.surface,
            borderTopColor: colors.border,
            paddingBottom: 4,
            height: 60,
          },
          tabBarLabelStyle: { fontSize: 10 },
          headerStyle: { backgroundColor: colors.primary },
          headerTintColor: "#fff",
          headerTitleStyle: { fontWeight: "700" },
        })}
      >
        <Tab.Screen name="Dashboard" component={DashboardScreen} />
        <Tab.Screen name="Transakcje" component={TransactionsScreen} />
        <Tab.Screen name="Budżety" component={BudgetsScreen} />
        <Tab.Screen name="Cele" component={SavingsGoalsScreen} />
        <Tab.Screen name="Rachunki" component={BillsScreen} />
        <Tab.Screen name="Zakupy" component={ShoppingScreen} />
        <Tab.Screen name="Konta" component={SplitsScreen} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}
