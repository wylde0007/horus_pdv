import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

const cartItems = [
  { item: "Café Tradicional 500g", qty: 2, price: "R$ 18,90", total: "R$ 37,80" },
  { item: "Leite Integral 1L", qty: 3, price: "R$ 5,49", total: "R$ 16,47" },
];

export default function SalesStartPage() {
  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Iniciar Vendas"
        description="Fluxo de venda rápida com adição de produtos e fechamento do pedido."
      />

      <section className="grid gap-4 xl:grid-cols-[2fr_1fr]">
        <article className="card p-4 md:p-5">
          <h2 className="text-base font-semibold text-text-primary">Adicionar produto</h2>
          <div className="mt-4 grid gap-3 md:grid-cols-3">
            <input className="input-field w-full md:col-span-2" placeholder="Buscar produto" />
            <input className="input-field w-full" placeholder="Quantidade" />
          </div>
          <div className="mt-4 flex justify-end">
            <button type="button" className="btn-success">
              Adicionar Produto
            </button>
          </div>

          <div className="mt-5 overflow-x-auto">
            <table className="w-full min-w-[680px] text-sm">
              <thead className="bg-bg-primary text-left text-text-secondary">
                <tr>
                  <th className="px-3 py-2">Item</th>
                  <th className="px-3 py-2">Qtd.</th>
                  <th className="px-3 py-2">Preço</th>
                  <th className="px-3 py-2">Total</th>
                </tr>
              </thead>
              <tbody>
                {cartItems.map((item) => (
                  <tr key={item.item} className="border-t border-border-primary">
                    <td className="px-3 py-2">{item.item}</td>
                    <td className="px-3 py-2">{item.qty}</td>
                    <td className="px-3 py-2">{item.price}</td>
                    <td className="px-3 py-2">{item.total}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </article>

        <article className="card p-4 md:p-5">
          <h2 className="text-base font-semibold text-text-primary">Resumo da venda</h2>
          <dl className="mt-4 space-y-2 text-sm">
            <div className="flex items-center justify-between">
              <dt className="text-text-secondary">Subtotal</dt>
              <dd className="font-semibold text-text-primary">R$ 54,27</dd>
            </div>
            <div className="flex items-center justify-between">
              <dt className="text-text-secondary">Desconto</dt>
              <dd className="font-semibold text-text-primary">R$ 0,00</dd>
            </div>
            <div className="flex items-center justify-between border-t border-border-primary pt-2">
              <dt className="text-text-secondary">Total</dt>
              <dd className="text-lg font-bold text-text-primary">R$ 54,27</dd>
            </div>
          </dl>

          <div className="mt-5 space-y-2">
            <button type="button" className="btn-primary w-full">
              Fechar Venda
            </button>
            <button type="button" className="btn-cancel w-full">
              Cancelar Venda
            </button>
          </div>
        </article>
      </section>
    </PageLayout>
  );
}
