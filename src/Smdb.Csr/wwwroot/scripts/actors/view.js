// Added by Copilot: actor view page JS for read-only details.
// Added by Copilot: provides link to the actor edit page.
import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";
(async function initActorView() {
  const id = getQueryParam("id");
  const statusEl = $("#status");
  if (!id) return renderStatus(statusEl, "err", "Missing ?id in URL.");
  try {
    const a = await apiFetch(`/actors/${encodeURIComponent(id)}`);
    $("#actor-id").textContent = a.id;
    $("#actor-name").textContent = a.name;
    $("#actor-year").textContent = a.birthYear;
    $("#actor-bio").textContent = a.biography || "—";
    $("#edit-link").href = `/actors/edit.html?id=${encodeURIComponent(a.id)}`;
    renderStatus(statusEl, "ok", "Actor loaded successfully.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load actor ${id}: ${err.message}`);
  }
})();
