/**
 * Arquivo: src/utils/inputMasks.ts
 * Objetivo: centraliza máscaras e normalizadores de entrada textual.
 */
export function onlyDigits(value: string) {
  return value.replace(/\D/g, "");
}

export function maskCpf(value: string) {
  const digits = onlyDigits(value).slice(0, 11);
  return digits
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d{1,2})$/, "$1-$2");
}

export function maskCnpj(value: string) {
  const digits = onlyDigits(value).slice(0, 14);
  return digits
    .replace(/^(\d{2})(\d)/, "$1.$2")
    .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/\.(\d{3})(\d)/, ".$1/$2")
    .replace(/(\d{4})(\d)/, "$1-$2");
}

export function maskCpfOrCnpj(value: string) {
  const digits = onlyDigits(value);
  if (digits.length <= 11) return maskCpf(digits);
  return maskCnpj(digits);
}

export function maskPhoneBr(value: string) {
  const digits = onlyDigits(value).slice(0, 11);
  if (digits.length <= 2) return digits;
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
  if (digits.length <= 10) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`;
  }
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`;
}

export function maskTelephoneBr(value: string) {
  const digits = onlyDigits(value).slice(0, 10);
  if (digits.length <= 2) return digits;
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`;
}

export function maskCellphoneBr(value: string) {
  const digits = onlyDigits(value).slice(0, 11);
  if (digits.length <= 2) return digits;
  if (digits.length <= 7) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`;
}

export function maskCep(value: string) {
  const digits = onlyDigits(value).slice(0, 8);
  return digits.replace(/(\d{5})(\d)/, "$1-$2");
}

export function maskRg(value: string) {
  const digits = onlyDigits(value).slice(0, 9);
  return digits
    .replace(/^(\d{2})(\d)/, "$1.$2")
    .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/(\d{3})(\d)/, "$1-$2");
}

export function maskDateBr(value: string) {
  const digits = onlyDigits(value).slice(0, 8);
  if (digits.length <= 2) return digits;
  if (digits.length <= 4) return `${digits.slice(0, 2)}/${digits.slice(2)}`;
  return `${digits.slice(0, 2)}/${digits.slice(2, 4)}/${digits.slice(4)}`;
}

export function maskMoneyBr(value: string) {
  const digits = onlyDigits(value).slice(0, 12);
  if (!digits) return "";
  const cents = Number(digits) / 100;
  return cents.toLocaleString("pt-BR", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
}

export function parseMoneyBr(value: string) {
  const normalized = value.replace(/\./g, "").replace(",", ".");
  const parsed = Number(normalized);
  return Number.isFinite(parsed) ? parsed : 0;
}

export function formatMoneyBr(value: number) {
  if (!Number.isFinite(value)) return "0,00";
  return value.toLocaleString("pt-BR", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
}

export function maskTime(value: string) {
  const digits = onlyDigits(value).slice(0, 4);
  if (digits.length <= 2) return digits;

  const hours = Number(digits.slice(0, 2));
  const minutes = Number(digits.slice(2, 4));
  const safeHours = Number.isFinite(hours) ? Math.min(hours, 23) : 0;
  const safeMinutes = Number.isFinite(minutes) ? Math.min(minutes, 59) : 0;
  return `${String(safeHours).padStart(2, "0")}:${String(safeMinutes).padStart(2, "0")}`;
}
