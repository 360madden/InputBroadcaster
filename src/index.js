// InputBroadcaster sample fix for mgt.clearMarks issue

function clearPerformanceMark(name) {
  if (typeof performance !== 'undefined' && typeof performance.clearMarks === 'function') {
    console.log(`Clearing performance mark: ${name}`);
    performance.clearMarks(name);
  } else {
    console.warn('performance.clearMarks is not available in this environment', performance);
  }
}

function safeClearMgtMarks(mgt) {
  // @microsoft/mgt has no clearMarks API; avoid calling this directly.
  if (mgt && typeof mgt.clearMarks === 'function') {
    mgt.clearMarks();
  } else {
    console.warn('mgt.clearMarks not supported; using performance.clearMarks fallback');
    clearPerformanceMark('InputBroadcaster');
  }
}

// Example usage
const mgt = {}; // real MGT object (from @microsoft/mgt) would be overloaded here.

safeClearMgtMarks(mgt);

console.log('InputBroadcaster is running.');
