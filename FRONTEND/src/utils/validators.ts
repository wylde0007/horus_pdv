import { onlyDigits } from "./inputMasks";

const repeatedDigits = (value: string) => /^(\d)\1+$/.test(value);

export function isValidCpf(rawCpf: string) {
  const cpf = onlyDigits(rawCpf);
  if (cpf.length !== 11 || repeatedDigits(cpf)) return false;

  let sum = 0;
  for (let i = 0; i < 9; i += 1) sum += Number(cpf[i]) * (10 - i);
  let digit = 11 - (sum % 11);
  if (digit >= 10) digit = 0;
  if (digit !== Number(cpf[9])) return false;

  sum = 0;
  for (let i = 0; i < 10; i += 1) sum += Number(cpf[i]) * (11 - i);
  digit = 11 - (sum % 11);
  if (digit >= 10) digit = 0;
  return digit === Number(cpf[10]);
}

export function isValidCnpj(rawCnpj: string) {
  const cnpj = onlyDigits(rawCnpj);
  if (cnpj.length !== 14 || repeatedDigits(cnpj)) return false;

  const calcDigit = (base: string, factors: number[]) => {
    const sum = base
      .split("")
      .reduce((acc, digit, index) => acc + Number(digit) * factors[index], 0);
    const remainder = sum % 11;
    return remainder < 2 ? 0 : 11 - remainder;
  };

  const base12 = cnpj.slice(0, 12);
  const d1 = calcDigit(base12, [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
  const d2 = calcDigit(`${base12}${d1}`, [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
  return cnpj.endsWith(`${d1}${d2}`);
}

export function isValidEmail(email: string) {
  if (!email.trim()) return true;
  return /\S+@\S+\.\S+/.test(email.trim());
}

export function parseBirthDateBr(rawValue: string) {
  const [day, month, year] = rawValue.split("/").map(Number);
  if (!day || !month || !year) return null;
  const date = new Date(year, month - 1, day);
  if (
    date.getFullYear() !== year ||
    date.getMonth() !== month - 1 ||
    date.getDate() !== day
  ) {
    return null;
  }
  return date;
}

export function getAgeFromBirthDate(rawValue: string) {
  const birthDate = parseBirthDateBr(rawValue);
  if (!birthDate) return null;

  const today = new Date();
  let age = today.getFullYear() - birthDate.getFullYear();
  const hasBirthdayPassed =
    today.getMonth() > birthDate.getMonth() ||
    (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate());

  if (!hasBirthdayPassed) age -= 1;
  if (age < 0 || age > 130) return null;
  return age;
}
