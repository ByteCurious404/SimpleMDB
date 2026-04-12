// Added by Copilot: shared frontend helpers for movies, actors, and users.
// Added by Copilot: captureActorForm and captureUserForm were added here.
export const API_BASE = "http://localhost:8080/api/v1";
export const $ = (sel, el = document) => el.querySelector(sel);
export const $$ = (sel, el = document) => Array.from(el.querySelectorAll(sel));
export const getQueryParam = (k) => new URLSearchParams(location.search).get(k);
function jsonHeaders() {
  return { "Content-Type": "application/json", Accept: "application/json" };
}

export async function apiFetch(path, opts = {}) {
  const url = path.startsWith("http") ? path : `${API_BASE}${path}`;
  const init = {
    ...opts,
    headers: { ...(opts.headers || {}), ...jsonHeaders() },
  };
  const res = await fetch(url, init);
  const text = await res.text();
  let payload = null;
  try {
    payload = text ? JSON.parse(text) : null;
  } catch {
    payload = text;
  }
  if (!res.ok) {
    const msg =
      (payload && (payload.message || payload.error)) ||
      `${res.status} ${res.statusText}`;
    const err = new Error(msg);
    err.status = res.status;
    err.payload = payload;
    throw err;
  }
  return payload;
}

export const apiCall = apiFetch;

export function renderStatus(el, type, message) {
  if (!el) return;
  el.className = `status ${type}`;
  el.textContent = message;
}

export function clearChildren(el) {
  //while (el.firstChild) el.removeChild(el.firstChild);
  el.replaceChildren();
}

export function captureMovieForm(form) {
  const title = form.title.value.trim();
  const year = Number(form.year.value);
  const description = form.description.value.trim();
  return { title, year, description };
}

export function captureActorForm(form) {
  const name = form.name.value.trim();
  const birthYear = Number(form.birthYear.value);
  const biography = form.biography.value.trim();
  return { name, birthYear, biography };
}

export function captureUserForm(form) {
  const username = form.username.value.trim();
  const email = form.email.value.trim();
  const fullName = form.fullName.value.trim();
  const birthYear = Number(form.birthYear.value);
  const bio = form.bio.value.trim();
  return { username, email, fullName, birthYear, bio };
}
