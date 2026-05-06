import { useEffect, useState } from "react";
import LoadingBar from "@/components/Loading/LoadingBar";
import { marketModuleService } from "@/services/api/marketModuleService";
import MarketModulePage, { type MarketModuleConfig } from "./MarketModulePage";

export default function MarketModuleRoutePage({ moduleId }: { moduleId: string }) {
  const [config, setConfig] = useState<MarketModuleConfig | null>(null);

  useEffect(() => {
    marketModuleService
      .get(moduleId)
      .then((data) => setConfig(data ?? null))
      .catch(() => setConfig(null));
  }, [moduleId]);

  if (!config) {
    return (
      <div className="flex min-h-[360px] items-center justify-center text-text-secondary">
        <LoadingBar />
      </div>
    );
  }

  return <MarketModulePage config={config} />;
}
