/**
 * Arquivo: src/pages/Admin/MyCompanyPage.tsx
 * Objetivo: centraliza configuracoes cadastrais e de contato da empresa.
 * Entradas esperadas: nao recebe props; renderiza formulario de dados institucionais.
 */
import { useState } from "react";
import PageHeader from "@/components/Admin/PageHeader";
import useInputMasks from "@/hooks/InputMasks/useInputMasks";
import PageLayout from "@/layout/PageLayout";
import { lookupAddressByCep, sanitizeCep } from "@/utils/cepLookup";

const UF_OPTIONS = [
  "AC",
  "AL",
  "AP",
  "AM",
  "BA",
  "CE",
  "DF",
  "ES",
  "GO",
  "MA",
  "MT",
  "MS",
  "MG",
  "PA",
  "PB",
  "PR",
  "PE",
  "PI",
  "RJ",
  "RN",
  "RS",
  "RO",
  "RR",
  "SC",
  "SP",
  "SE",
  "TO",
] as const;

export default function MyCompanyPage() {
  const { maskCep, maskCnpj, maskPhoneBr } = useInputMasks();

  const [cnpj, setCnpj] = useState("");
  const [telSac, setTelSac] = useState("");
  const [telefone, setTelefone] = useState("");
  const [celular, setCelular] = useState("");
  const [cep, setCep] = useState("");
  const [address, setAddress] = useState("");
  const [number, setNumber] = useState("");
  const [neighborhood, setNeighborhood] = useState("");
  const [city, setCity] = useState("");
  const [uf, setUf] = useState("SP");
  const [complement, setComplement] = useState("");
  const [cepLookupLoading, setCepLookupLoading] = useState(false);
  const [cepLookupError, setCepLookupError] = useState("");

  const handleCepLookup = async () => {
    const rawCep = sanitizeCep(cep);

    if (rawCep.length === 0) {
      setCepLookupError("");
      return;
    }

    if (rawCep.length !== 8) {
      setCepLookupError("CEP inválido. Informe 8 dígitos.");
      return;
    }

    setCepLookupLoading(true);
    setCepLookupError("");

    const result = await lookupAddressByCep(rawCep);

    setCepLookupLoading(false);

    if (!result.success) {
      setCepLookupError("Verifique o CEP e tente novamente.");
      return;
    }

    setCep(maskCep(result.data.cep || rawCep));
    setAddress(result.data.endereco || "");
    setNeighborhood(result.data.bairro || "");
    setCity(result.data.cidade || "");
    if (result.data.estado) setUf(result.data.estado);
    setComplement(result.data.complemento || "");
  };

  return (
    <PageLayout className="space-y-4 py-4 md:py-6 lg:py-8">
      <PageHeader
        title="Minha Empresa"
        description="Dados cadastrais e de contato da empresa."
      />

      <section className="card rounded-2xl p-4 md:p-5">
        <form className="grid gap-4 md:grid-cols-12">
          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Nome Fantasia
            </span>
            <input className="input-field w-full" placeholder="Nome fantasia" />
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Razão Social
            </span>
            <input className="input-field w-full" placeholder="Razão social" />
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              CNPJ
            </span>
            <input
              className="input-field w-full"
              value={cnpj}
              onChange={(event) => setCnpj(maskCnpj(event.target.value))}
              placeholder="00.000.000/0000-00"
            />
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Inscrição Estadual
            </span>
            <input
              className="input-field w-full"
              placeholder="Inscrição estadual"
            />
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Site
            </span>
            <input
              className="input-field w-full"
              placeholder="https://www.seusite.com.br"
            />
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Email de Contato
            </span>
            <input
              className="input-field w-full"
              placeholder="email@empresa.com.br"
            />
          </label>

          <label className="block md:col-span-4">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Tel SAC
            </span>
            <input
              className="input-field w-full"
              value={telSac}
              onChange={(event) => setTelSac(maskPhoneBr(event.target.value))}
              placeholder="(00) 00000-0000"
            />
          </label>

          <label className="block md:col-span-4">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Telefone
            </span>
            <input
              className="input-field w-full"
              value={telefone}
              onChange={(event) => setTelefone(maskPhoneBr(event.target.value))}
              placeholder="(00) 00000-0000"
            />
          </label>

          <label className="block md:col-span-4">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Celular
            </span>
            <input
              className="input-field w-full"
              value={celular}
              onChange={(event) => setCelular(maskPhoneBr(event.target.value))}
              placeholder="(00) 00000-0000"
            />
          </label>

          <label className="block md:col-span-3">
            <span className="mb-1.5 block text-sm text-text-secondary">
              CEP
            </span>
            <input
              className="input-field w-full"
              value={cep}
              onChange={(event) => setCep(maskCep(event.target.value))}
              onBlur={handleCepLookup}
              placeholder="00000-000"
            />
            {cepLookupLoading ? (
              <p className="mt-1 text-xs text-text-secondary">Consultando CEP...</p>
            ) : null}
            {cepLookupError ? (
              <p className="mt-1 text-xs text-primary">{cepLookupError}</p>
            ) : null}
          </label>

          <label className="block md:col-span-6">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Endereço
            </span>
            <input
              className="input-field w-full"
              value={address}
              onChange={(event) => setAddress(event.target.value)}
              placeholder="Rua, avenida, alameda..."
            />
          </label>

          <label className="block md:col-span-3">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Número
            </span>
            <input
              className="input-field w-full"
              value={number}
              onChange={(event) => setNumber(event.target.value)}
              placeholder="Número"
            />
          </label>

          <label className="block md:col-span-3">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Bairro
            </span>
            <input
              className="input-field w-full"
              value={neighborhood}
              onChange={(event) => setNeighborhood(event.target.value)}
              placeholder="Bairro"
            />
          </label>

          <label className="block md:col-span-4">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Cidade
            </span>
            <input
              className="input-field w-full"
              value={city}
              onChange={(event) => setCity(event.target.value)}
              placeholder="Cidade"
            />
          </label>

          <label className="block md:col-span-2">
            <span className="mb-1.5 block text-sm text-text-secondary">UF</span>
            <select
              className="select-field w-full"
              value={uf}
              onChange={(event) => setUf(event.target.value)}
            >
              {UF_OPTIONS.map((option) => (
                <option key={option} value={option}>
                  {option}
                </option>
              ))}
            </select>
          </label>

          <label className="block md:col-span-3">
            <span className="mb-1.5 block text-sm text-text-secondary">
              Complemento
            </span>
            <input
              className="input-field w-full"
              value={complement}
              onChange={(event) => setComplement(event.target.value)}
              placeholder="Apto, bloco..."
            />
          </label>

          <div className="flex justify-end md:col-span-12">
            <button type="button" className="btn-primary">
              Salvar dados da empresa
            </button>
          </div>
        </form>
      </section>
    </PageLayout>
  );
}
