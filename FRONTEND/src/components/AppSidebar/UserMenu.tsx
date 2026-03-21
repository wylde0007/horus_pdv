/**
 * Arquivo: src/components/AppSidebar/UserMenu.tsx
 * Objetivo: exibe avatar e menu de ações do usuário no rodapé da sidebar.
 * Entradas esperadas: dados do usuário e callbacks para perfil, configurações e logout.
 */
import { ChevronUp, LogOut, Settings, User } from "lucide-react";
import { useEffect, useRef, useState } from "react";

type UserMenuProps = {
  collapsed: boolean;
  currentUserName: string;
  currentUserPermission: string;
  avatarUrl: string | null;
  onOpenProfile: () => void;
  onOpenSettings: () => void;
  onLogout: () => void;
};

export default function UserMenu({
  collapsed,
  currentUserName,
  currentUserPermission,
  avatarUrl,
  onOpenProfile,
  onOpenSettings,
  onLogout,
}: UserMenuProps) {
  // Controla abertura do dropdown de usuário.
  const [showUserMenu, setShowUserMenu] = useState(false);
  const userMenuRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (!userMenuRef.current?.contains(event.target as Node)) {
        setShowUserMenu(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const userInitials = currentUserName
    // Gera avatar textual com no máximo duas iniciais.
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((part) => part[0]?.toUpperCase() ?? "")
    .join("");

  return (
    <div className="relative" ref={userMenuRef}>
      <button
        type="button"
        onClick={() => setShowUserMenu((current) => !current)}
        className="w-full flex items-center gap-3 rounded-xl px-2 py-2 hover:bg-accent/10 focus-visible:outline-none focus-visible:shadow-[0_0_0_3px_rgba(46,191,244,0.22)]"
      >
        <div className="w-9 h-9 rounded-full bg-gradient-to-br from-accent to-secondary text-white text-xs font-semibold flex items-center justify-center shadow-sm overflow-hidden">
          {avatarUrl ? (
            <img
              src={avatarUrl}
              alt={`Avatar de ${currentUserName}`}
              className="h-full w-full object-cover"
            />
          ) : (
            userInitials || "AD"
          )}
        </div>

        {!collapsed && (
          <>
            <div className="flex-1 text-left">
              <span className="block text-sm font-semibold text-text-primary">
                {currentUserName}
              </span>
              <span className="block text-[11px] text-text-secondary">
                {currentUserPermission}
              </span>
            </div>
            <ChevronUp
              size={16}
              className={`text-text-secondary transition-transform ${showUserMenu ? "rotate-180" : ""}`}
            />
          </>
        )}
      </button>

      {showUserMenu && (
        <div
          className={`absolute z-20 bottom-12 bg-bg-light border border-border-secondary rounded-xl shadow-lg p-1.5 min-w-52 ${
            collapsed ? "left-0" : "right-0"
          }`}
        >
          <button
            type="button"
            onClick={() => {
              onOpenProfile();
              setShowUserMenu(false);
            }}
            className="w-full flex items-center gap-2 px-3 py-2 text-sm rounded-lg hover:bg-accent/10"
          >
            <User size={16} />
            Meu Perfil
          </button>

          <button
            type="button"
            onClick={() => {
              onOpenSettings();
              setShowUserMenu(false);
            }}
            className="w-full flex items-center gap-2 px-3 py-2 text-sm rounded-lg hover:bg-accent/10"
          >
            <Settings size={16} />
            Configurações
          </button>

          <button
            type="button"
            onClick={() => {
              onLogout();
              setShowUserMenu(false);
            }}
            className="w-full flex items-center gap-2 px-3 py-2 text-sm rounded-lg hover:bg-accent/10 text-primary"
          >
            <LogOut size={16} />
            Sair
          </button>
        </div>
      )}
    </div>
  );
}
