import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

export default function LicenseDetailsPage() {
  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Detalhes da Licença"
        description="Informações da licença ativa e ações de renovação."
      />

      <section className="card p-4 md:p-5">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[680px] text-sm">
            <thead className="bg-bg-primary text-left text-text-secondary">
              <tr>
                <th className="px-4 py-3">Usuário da Licença</th>
                <th className="px-4 py-3">Plano</th>
                <th className="px-4 py-3">Validade</th>
                <th className="px-4 py-3">Status</th>
              </tr>
            </thead>
            <tbody>
              <tr className="border-t border-border-primary">
                <td className="px-4 py-3">flavio@hpdv.com.br</td>
                <td className="px-4 py-3">Hórus PDV Pro</td>
                <td className="px-4 py-3">30/04/2026</td>
                <td className="px-4 py-3">
                  <span className="rounded-full bg-success/15 px-2 py-1 text-xs font-semibold text-success">
                    Ativa
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <p className="mt-4 text-sm text-text-secondary">
          Para renovar sua licença, gere a chave PIX e conclua o pagamento.
        </p>
        <div className="mt-3">
          <button type="button" className="btn-primary">
            Gerar chave PIX
          </button>
        </div>
      </section>
    </PageLayout>
  );
}
