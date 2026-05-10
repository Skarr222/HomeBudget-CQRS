import { AccountType } from "./enums";

export interface AccountDto {
  id: number;
  name: string;
  type: AccountType;
  balance: number;
  color: string;
  icon: string;
  userId: number;
  userName: string;
}

export interface CreateAccountDto {
  name: string;
  type: AccountType;
  balance: number;
  color: string;
  icon: string;
  userId: number;
}

export interface UpdateAccountDto {
  id: number;
  name: string;
  type: AccountType;
  balance: number;
  color: string;
  icon: string;
}
