import { useState } from "react";
import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

type ChangePasswordResult = {
  success: boolean;
  message: string;
};

type ChangePasswordPageProps = {
  onChangePassword: (currentPassword: string, nextPassword: string) => ChangePasswordResult;
};

export default function ChangePasswordPage({
  onChangePassword,
}: ChangePasswordPageProps) {
  const [currentPassword, setCurrentPassword] = useState("");
  const [nextPassword, setNextPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [feedback, setFeedback] = useState<ChangePasswordResult | null>(null);

  const handleSubmit = () => {
    if (!currentPassword || !nextPassword || !confirmPassword) {
      setFeedback({ success: false, message: "Preencha todos os campos." });
      return;
    }

    if (nextPassword !== confirmPassword) {
      setFeedback({ success: false, message: "A confirmação de senha não confere." });
      return;
    }

    const result = onChangePassword(currentPassword, nextPassword);
    setFeedback(result);

    if (result.success) {
      setCurrentPassword("");
      setNextPassword("");
      setConfirmPassword("");
    }
  };

  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Alterar Senha"
        description="Atualize sua senha de acesso ao painel administrativo."
      />

      <section className="card max-w-2xl p-4 md:p-5">
        <div className="space-y-3">
          <input
            className="input-field w-full"
            type="password"
            placeholder="Senha atual"
            value={currentPassword}
            onChange={(event) => setCurrentPassword(event.target.value)}
          />
          <input
            className="input-field w-full"
            type="password"
            placeholder="Nova senha"
            value={nextPassword}
            onChange={(event) => setNextPassword(event.target.value)}
          />
          <input
            className="input-field w-full"
            type="password"
            placeholder="Confirmar nova senha"
            value={confirmPassword}
            onChange={(event) => setConfirmPassword(event.target.value)}
          />
        </div>

        {feedback ? (
          <p
            className={`mt-3 rounded-lg border px-3 py-2 text-sm ${
              feedback.success
                ? "border-success/30 bg-success/10 text-success"
                : "border-primary/30 bg-primary/10 text-primary"
            }`}
          >
            {feedback.message}
          </p>
        ) : null}

        <div className="mt-4 flex justify-end">
          <button type="button" className="btn-primary" onClick={handleSubmit}>
            Alterar
          </button>
        </div>
      </section>
    </PageLayout>
  );
}
