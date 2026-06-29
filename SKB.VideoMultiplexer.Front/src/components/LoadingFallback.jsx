import './LoadingFallback.css'

export default function LoadingFallback() {
  return (
    <div className="loading-fallback">
      <div className="loading-spinner" />
      <p>Authenticating…</p>
    </div>
  )
}
