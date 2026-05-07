import type { PageKey } from "@/components/AppSidebar/AppSidebar";

export const APP_NAVIGATE_EVENT = "horuspdv:navigate-page";
export const APP_OPEN_TOUR_EVENT = "horuspdv:open-guided-tour";

export type AppNavigateDetail = {
  page: PageKey;
};

export function navigateToAppPage(page: PageKey) {
  window.dispatchEvent(
    new CustomEvent<AppNavigateDetail>(APP_NAVIGATE_EVENT, {
      detail: { page },
    }),
  );
}

export function openGuidedTour() {
  window.dispatchEvent(new Event(APP_OPEN_TOUR_EVENT));
}
