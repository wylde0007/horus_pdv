/**
 * Arquivo: src/hooks/useInputMasks.ts
 * Objetivo: expõe máscaras de input de forma centralizada para componentes.
 */
import { useMemo } from "react";
import {
  formatMoneyBr,
  maskCellphoneBr,
  maskCep,
  maskCnpj,
  maskCpf,
  maskCpfOrCnpj,
  maskDateBr,
  maskMoneyBr,
  maskPhoneBr,
  maskRg,
  maskTelephoneBr,
  maskTime,
  onlyDigits,
  parseMoneyBr,
} from "@/utils/inputMasks";

export default function useInputMasks() {
  return useMemo(
    () => ({
      onlyDigits,
      maskCpf,
      maskCnpj,
      maskCpfOrCnpj,
      maskPhoneBr,
      maskTelephoneBr,
      maskCellphoneBr,
      maskCep,
      maskRg,
      maskDateBr,
      maskMoneyBr,
      parseMoneyBr,
      formatMoneyBr,
      maskTime,
    }),
    [],
  );
}
