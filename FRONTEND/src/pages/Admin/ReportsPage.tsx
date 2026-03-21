import { useState } from "react";
import PageHeader from "@/components/Admin/PageHeader";
import { DatePickerField } from "@/components/Form";
import PageLayout from "@/layout/PageLayout";

const reportTypes = [
  "Relatório de Cliente",
  "Relatório de Produto",
  "Relatório de Fornecedor",
  "Histórico de Vendas",
];

export default function ReportsPage() {
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Relatórios"
        description="Geração de relatórios operacionais por tipo de cadastro e vendas."
      />

      <section className="card p-4 md:p-5">
        <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-4">
          <select className="select-field w-full">
            <option>Tipo de Relatório</option>
            {reportTypes.map((type) => (
              <option key={type}>{type}</option>
            ))}
          </select>
          <DatePickerField
            value={startDate}
            onChange={setStartDate}
            className="w-full"
            placeholder="Data inicial"
            format="br"
          />
          <DatePickerField
            value={endDate}
            onChange={setEndDate}
            className="w-full"
            placeholder="Data final"
            format="br"
          />
          <button type="button" className="btn-primary">
            Gerar Relatório
          </button>
        </div>
      </section>

      <section className="grid gap-3 sm:grid-cols-2 xl:grid-cols-4">
        {reportTypes.map((type) => (
          <article key={type} className="card p-4">
            <p className="text-xs font-semibold uppercase tracking-[0.08em] text-text-secondary">
              {type}
            </p>
            <p className="mt-2 text-sm text-text-primary">
              Última geração em 21/03/2026 às 16:10.
            </p>
          </article>
        ))}
      </section>
    </PageLayout>
  );
}
