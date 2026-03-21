import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

const users = [
  { name: "Flávio Oliveira", email: "flavio@hpdv.com.br", role: "administrador" },
  { name: "Maria Santos", email: "maria@hpdv.com.br", role: "cadastro" },
  { name: "João Costa", email: "joao@hpdv.com.br", role: "venda" },
];

export default function UserAccountsPage() {
  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Contas de Usuários"
        description="Criação e atualização de contas com níveis de permissão do sistema."
      />

      <section className="card p-4 md:p-5">
        <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-4">
          <input className="input-field w-full" placeholder="Nome do usuário" />
          <input className="input-field w-full" placeholder="E-mail de acesso" />
          <select className="select-field w-full">
            <option>administrador</option>
            <option>cadastro</option>
            <option>venda</option>
          </select>
          <button type="button" className="btn-primary">
            Salvar
          </button>
        </div>
      </section>

      <section className="card overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[720px] text-sm">
            <thead className="bg-bg-primary text-left text-text-secondary">
              <tr>
                <th className="px-4 py-3">Usuário</th>
                <th className="px-4 py-3">E-mail</th>
                <th className="px-4 py-3">Permissão</th>
              </tr>
            </thead>
            <tbody>
              {users.map((user) => (
                <tr key={user.email} className="border-t border-border-primary">
                  <td className="px-4 py-3">{user.name}</td>
                  <td className="px-4 py-3">{user.email}</td>
                  <td className="px-4 py-3">
                    <span className="rounded-full bg-secondary/12 px-2 py-1 text-xs font-semibold text-secondary">
                      {user.role}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </PageLayout>
  );
}
