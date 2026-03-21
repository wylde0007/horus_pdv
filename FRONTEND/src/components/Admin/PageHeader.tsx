/**
 * Arquivo: src/components/Admin/PageHeader.tsx
 * Objetivo: renderiza titulo, descricao e acao principal das paginas administrativas.
 * Entradas esperadas: titulo, descricao e opcional de acao no topo da tela.
 */
import type { ReactNode } from "react";

type PageHeaderProps = {
  title: string;
  description: string;
  action?: ReactNode;
};

export default function PageHeader({
  title,
  description,
  action,
}: PageHeaderProps) {
  return (
    <header className="mb-5 flex flex-wrap items-start justify-between gap-3">
      <div>
        <h1 className="text-2xl font-bold text-text-primary">{title}</h1>
        <p className="mt-1 text-sm text-text-secondary">{description}</p>
      </div>
      {action ? <div>{action}</div> : null}
    </header>
  );
}
