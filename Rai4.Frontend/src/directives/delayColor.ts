import type { Directive } from "vue";

export const vDelayColor: Directive<HTMLElement, number> = {
  mounted(el, binding) {
    const delayInSeconds = binding.value;

    // Remove any existing delay classes
    el.classList.remove(
      "delay-late",
      "delay-very-late",
      "delay-early",
      "delay-on-time"
    );

    if (delayInSeconds > 300) {
      // More than 5 minutes late - very late (dark red)
      el.classList.add("delay-very-late");
    } else if (delayInSeconds > 60) {
      // More than 1 minute late - late (red)
      el.classList.add("delay-late");
    } else if (delayInSeconds < -60) {
      // More than 1 minute early (light blue)
      el.classList.add("delay-early");
    } else {
      // On time (within 1 minute) - green
      el.classList.add("delay-on-time");
    }
  },
  updated(el, binding) {
    // Reapply when value changes
    const delayInSeconds = binding.value;

    el.classList.remove(
      "delay-late",
      "delay-very-late",
      "delay-early",
      "delay-on-time"
    );

    if (delayInSeconds > 300) {
      el.classList.add("delay-very-late");
    } else if (delayInSeconds > 60) {
      el.classList.add("delay-late");
    } else if (delayInSeconds < -60) {
      el.classList.add("delay-early");
    } else {
      el.classList.add("delay-on-time");
    }
  },
};
