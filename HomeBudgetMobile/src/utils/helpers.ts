export const colors = {
  primary: "#6366F1",
  primaryLight: "#818CF8",
  primaryDark: "#4F46E5",
  success: "#22C55E",
  successLight: "#BBF7D0",
  danger: "#EF4444",
  dangerLight: "#FECACA",
  warning: "#F59E0B",
  warningLight: "#FEF3C7",
  background: "#F8FAFC",
  surface: "#FFFFFF",
  surfaceSecondary: "#F1F5F9",
  text: "#0F172A",
  textSecondary: "#64748B",
  textMuted: "#94A3B8",
  border: "#E2E8F0",
  income: "#22C55E",
  expense: "#EF4444",
};

export const formatCurrency = (amount: number): string =>
  `${amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, " ")} zł`;

export const formatDate = (date: string): string =>
  new Date(date).toLocaleDateString("pl-PL", {
    day: "numeric",
    month: "short",
    year: "numeric",
  });

export const formatShortDate = (date: string): string =>
  new Date(date).toLocaleDateString("pl-PL", {
    day: "numeric",
    month: "short",
  });

export const getMonthName = (month: number): string => {
  const months = [
    "Styczeń",
    "Luty",
    "Marzec",
    "Kwiecień",
    "Maj",
    "Czerwiec",
    "Lipiec",
    "Sierpień",
    "Wrzesień",
    "Październik",
    "Listopad",
    "Grudzień",
  ];
  return months[month - 1] || "";
};
