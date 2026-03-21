import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

const sales = [
  { id: "VD-10421", client: "Ana Martins", total: "R$ 342,70", date: "21/03/2026 14:12" },
  { id: "VD-10420", client: "Lucas Souza", total: "R$ 89,90", date: "21/03/2026 13:42" },
  { id: "VD-10419", client: "Beatriz Lima", total: "R$ 167,20", date: "21/03/2026 12:55" },
];

export default function SalesHistoryPage() {
  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Histórico de Vendas"
        description="Consulta rápida de vendas concluídas e seus respectivos totais."
      />

      <section className="card p-4 md:p-5">
        <div className="grid gap-3 md:grid-cols-3">
          <input className="input-field w-full" placeholder="Buscar por número da venda" />
          <input className="input-field w-full" placeholder="Buscar por cliente" />
          <button type="button" className="btn-outline-secondary">
            Filtrar
          </button>
        </div>
      </section>

      <section className="card overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[720px] text-sm">
            <thead className="bg-bg-primary text-left text-text-secondary">
              <tr>
                <th className="px-4 py-3">Número da Venda</th>
                <th className="px-4 py-3">Cliente</th>
                <th className="px-4 py-3">Total</th>
                <th className="px-4 py-3">Data da Venda</th>
              </tr>
            </thead>
            <tbody>
              {sales.map((sale) => (
                <tr key={sale.id} className="border-t border-border-primary">
                  <td className="px-4 py-3 font-semibold text-text-primary">{sale.id}</td>
                  <td className="px-4 py-3">{sale.client}</td>
                  <td className="px-4 py-3">{sale.total}</td>
                  <td className="px-4 py-3">{sale.date}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </PageLayout>
  );
}
