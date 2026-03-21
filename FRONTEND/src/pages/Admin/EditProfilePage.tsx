import { Camera } from "lucide-react";
import { useRef } from "react";
import PageHeader from "@/components/Admin/PageHeader";
import PageLayout from "@/layout/PageLayout";

type EditProfilePageProps = {
  userName: string;
  userEmail: string;
  userRole: string;
  userAvatarUrl: string | null;
  onUploadAvatar: (file: File) => void;
};

export default function EditProfilePage({
  userName,
  userEmail,
  userRole,
  userAvatarUrl,
  onUploadAvatar,
}: EditProfilePageProps) {
  const inputFileRef = useRef<HTMLInputElement | null>(null);

  const initials = userName
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((part) => part[0]?.toUpperCase() ?? "")
    .join("");

  return (
    <PageLayout className="space-y-4 py-4 md:space-y-6 md:py-6 lg:py-8">
      <PageHeader
        title="Editar Perfil"
        description="Atualize seus dados de identificação para o painel administrativo."
      />

      <section className="card p-4 md:p-5">
        <div className="flex flex-col gap-5 md:flex-row md:items-center">
          <div className="relative">
            <div className="h-20 w-20 overflow-hidden rounded-full bg-gradient-to-br from-accent to-secondary text-xl font-bold text-white shadow-sm">
              {userAvatarUrl ? (
                <img
                  src={userAvatarUrl}
                  alt={`Avatar de ${userName}`}
                  className="h-full w-full object-cover"
                />
              ) : (
                <div className="flex h-full w-full items-center justify-center">
                  {initials || "HP"}
                </div>
              )}
            </div>
            <button
              type="button"
              onClick={() => inputFileRef.current?.click()}
              className="absolute -right-1 -bottom-1 inline-flex h-10 w-10 items-center justify-center rounded-full border border-border-primary bg-white text-accent shadow-sm transition hover:bg-accent/10"
            >
              <Camera size={16} />
            </button>
            <input
              ref={inputFileRef}
              type="file"
              accept="image/*"
              className="hidden"
              onChange={(event) => {
                const selectedFile = event.target.files?.[0];
                if (selectedFile) onUploadAvatar(selectedFile);
              }}
            />
          </div>

          <div className="grid w-full gap-3 md:grid-cols-2">
            <input className="input-field w-full" defaultValue={userName} />
            <input className="input-field w-full" defaultValue={userEmail} />
            <input className="input-field w-full md:col-span-2" defaultValue={userRole} />
          </div>
        </div>

        <div className="mt-4 flex justify-end">
          <button type="button" className="btn-primary">
            Salvar
          </button>
        </div>
      </section>
    </PageLayout>
  );
}
