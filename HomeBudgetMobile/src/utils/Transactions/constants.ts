import { PaymentMethod } from "../../models/types";

export const PAYMENT_METHODS: PaymentMethod[] = [
  PaymentMethod.Card,
  PaymentMethod.Cash,
  PaymentMethod.BLIK,
  PaymentMethod.Transfer,
];
