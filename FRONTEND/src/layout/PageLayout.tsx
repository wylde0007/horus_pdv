/**
 * Arquivo: src/layout/PageLayout.tsx
 * Objetivo: padroniza largura máxima e espaçamento lateral das páginas.
 * Entradas esperadas: children e opcional de tamanho visual do container.
 */
import type { ReactNode } from "react";

type PageLayoutSize = "default" | "wide";

type PageLayoutProps = {
  children: ReactNode;
  size?: PageLayoutSize;
  className?: string;
};

const SIZE_CLASS: Record<PageLayoutSize, string> = {
  default: "max-w-[1200px]",
  wide: "max-w-[1320px]",
};

export default function PageLayout({
  children,
  size = "default",
  className = "",
}: PageLayoutProps) {
  return (
    <section
      className={`mx-auto w-full px-3 sm:px-4 md:px-6 lg:px-4 xl:px-6 ${SIZE_CLASS[size]} ${className}`.trim()}
    >
      {children}
    </section>
  );
}
