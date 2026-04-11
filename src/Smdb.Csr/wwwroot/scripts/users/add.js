// Added by Copilot: user add page JS for creating new users.
// Added by Copilot: validates required fields before sending the request.
import { $, apiFetch, renderStatus, captureUserForm } from "/scripts/common.js";
(async function initUserAdd() {
  const form = $("#user-form");
  const statusEl = $("#status");
  renderStatus(statusEl, "ok", "New user. Fill in the fields and save.");
  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const payload = captureUserForm(form);
    if (!payload.username || !payload.email || !payload.fullName) {
      renderStatus(statusEl, "err", "Username, email, and full name are required.");
      return;
    }
    try {
      const created = await apiFetch("/users", {
        method: "POST",
        body: JSON.stringify(payload),
      });
      renderStatus(statusEl, "ok", `Created user #${created.id} "${created.username}".`);
      form.reset();
    } catch (err) {
      renderStatus(statusEl, "err", `Create failed: ${err.message}`);
    }
  });
})();
