// Added by Copilot: actor add page JS for creating new actors.
// Added by Copilot: uses shared form helper to keep UI consistent.
import { $, apiFetch, renderStatus, captureActorForm } from "/scripts/common.js";
(async function initActorAdd() {
  const form = $("#actor-form");
  const statusEl = $("#status");
  renderStatus(statusEl, "ok", "New actor. Fill the form and save.");
  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const payload = captureActorForm(form);
    if (!payload.name) {
      renderStatus(statusEl, "err", "Actor name is required.");
      return;
    }
    if (payload.name.length > 100) {
      renderStatus(statusEl, "err", "Name should not be longer than 100 characters.");
      return;
    }
    try {
      const created = await apiFetch("/actors", {
        method: "POST",
        body: JSON.stringify(payload),
      });
      renderStatus(statusEl, "ok", `Created actor #${created.id} "${created.name}".`);
      form.reset();
    } catch (err) {
      renderStatus(statusEl, "err", `Create failed: ${err.message}`);
    }
  });
})();
