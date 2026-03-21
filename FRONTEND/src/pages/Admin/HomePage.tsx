import { FileText, History, ShoppingCart, UserRoundPlus } from "lucide-react";
import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

const cards = [
  { label: "Vendas do dia", value: "42", helper: "+12% vs ontem" },
  { label: "Ticket médio", value: "R$ 186,30", helper: "+4,2% vs ontem" },
  { label: "Clientes atendidos", value: "31", helper: "Pico às 15:00" },
  { label: "Pedidos abertos", value: "6", helper: "2 aguardando pagamento" },
];

const shortcuts = [
  { title: "Cadastro de Cliente", icon: UserRoundPlus },
  { title: "Histórico de Vendas", icon: History },
  { title: "Relatórios", icon: FileText },
  { title: "Iniciar Vendas", icon: ShoppingCart },
];

export default function HomePage() {
  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Home"
        description="Visão geral da operação do Hórus PDV em tempo real."
      />

      <section className="grid gap-3 sm:grid-cols-2 xl:grid-cols-4">
        {cards.map((card) => (
          <article key={card.label} className="card p-4">
            <p className="text-xs font-semibold uppercase tracking-[0.08em] text-text-secondary">
              {card.label}
            </p>
            <p className="mt-2 text-2xl font-bold text-text-primary">{card.value}</p>
            <p className="mt-1 text-xs text-text-secondary">{card.helper}</p>
          </article>
        ))}
      </section>

      <section className="card p-4 md:p-5">
        <h2 className="text-base font-semibold text-text-primary">Ações rápidas</h2>
        <div className="mt-4 grid gap-3 sm:grid-cols-2 lg:grid-cols-4">
          {shortcuts.map((shortcut) => {
            const Icon = shortcut.icon;
            return (
              <button
                key={shortcut.title}
                type="button"
                className="group rounded-xl border border-border-primary bg-bg-primary p-4 text-left transition hover:border-secondary/40 hover:bg-accent/10"
              >
                <span className="inline-flex h-9 w-9 items-center justify-center rounded-lg bg-secondary/10 text-secondary">
                  <Icon size={18} />
                </span>
                <p className="mt-3 text-sm font-semibold text-text-primary group-hover:text-secondary">
                  {shortcut.title}
                </p>
              </button>
            );
          })}
        </div>
      </section>
    </PageLayout>
  );
}
