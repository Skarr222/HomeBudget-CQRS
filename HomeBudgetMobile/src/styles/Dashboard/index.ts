import { StyleSheet } from "react-native";
import { colors } from "../../utils/helpers";

export const styles = StyleSheet.create({
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

  accountsContent: { gap: 10 },
  accountCardBorder: { borderLeftWidth: 4 },
  sectionLast: { marginBottom: 32 },
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
  txIconText: { fontWeight: "700", fontSize: 12 },
});
