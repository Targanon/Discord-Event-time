# Copilot Instructions

## Project Guidelines
- For the planned web app/PWA work on this repo, use version 1.7.0.0 to match the updated desktop app version.
- Provide direct implementation guidance for getting the PWA working, as the user prefers this approach over step-by-step explanations.
- For the PWA UI, prefer a scrollable selector-style date/time input instead of the browser datetime-local picker because the current dropdown blocks the Generate button and is hard to use. The first implementation of the scrollable selector failed at runtime and should be replaced with a more robust approach. Postpone a dedicated mobile-only picker.
- Use the web app title/branding 'Targs Discord Local Time Generator'.
- Use a dark-only theme for the web app.
- Ensure the web app scales better across platforms and reduce the overall UI size significantly, especially for mobile, by approximately 50%.
- Maintain responsive behavior split by screen size: keep the compact desktop scaling, but implement a different, better mobile layout instead of the current tall multi-section flow.
- Confirmed UI preferences include a smaller header, collapsed how-to, collapsed preview, hiding the output box until Generate is clicked (making it expandable), and keeping the desktop layout separate and wider.