/**
 * Arquivo: src/pages/Admin/SettingsPage.tsx
 * Objetivo: renderiza página de configurações com tema e segurança de sessões.
 * Entradas esperadas: estado do tema e callback para alternância.
 */
import { useEffect, useMemo, useState } from "react";
import {
  SecuritySessionsCard,
  ThemeSettingsCard,
  type ActiveSession,
} from "@/components/SettingsPage";
import PageLayout from "@/layout/PageLayout";
import { sessionService } from "@/services/api/sessionService";

type ThemeMode = "light" | "dark";

type SettingsPageProps = {
  themeMode: ThemeMode;
  onToggleTheme: () => void;
};

export default function SettingsPage({
  themeMode,
  onToggleTheme,
}: SettingsPageProps) {
  const [sessions, setSessions] = useState<ActiveSession[]>([]);
  const [isLoading] = useState(false);

  useEffect(() => {
    sessionService.list().then(setSessions).catch(() => setSessions([]));
  }, []);

  const hasOtherSessions = useMemo(
    () => sessions.some((session) => !session.current),
    [sessions],
  );

  const handleTerminateSession = (sessionId: string) => {
    setSessions((current) => current.filter((item) => item.id !== sessionId));
  };

  const handleTerminateOtherSessions = () => {
    if (!hasOtherSessions) return;
    setSessions((current) => current.filter((item) => item.current));
  };

  return (
    <div className="flex-1 py-4 md:py-6 lg:py-8">
      <PageLayout>
        <div className="card overflow-hidden">
          <div className="border-b border-border-primary bg-gradient-to-r from-secondary/8 via-bg-light to-accent/8 px-6 py-5">
            <h2 className="text-2xl font-semibold text-text-primary">Configurações</h2>
            <p className="mt-1 text-sm text-text-secondary">
              Personalize preferências visuais da sua experiência no sistema.
            </p>
          </div>

          <div className="space-y-4 px-6 py-6">
            <ThemeSettingsCard themeMode={themeMode} onToggleTheme={onToggleTheme} />
            <SecuritySessionsCard
              sessions={sessions}
              isLoading={isLoading}
              onTerminateSession={handleTerminateSession}
              onTerminateOtherSessions={handleTerminateOtherSessions}
            />
          </div>
        </div>
      </PageLayout>
    </div>
  );
}
