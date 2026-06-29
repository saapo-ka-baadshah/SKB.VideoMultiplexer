import { Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import LoadingFallback from './LoadingFallback'

export default function ProtectedRoute({ children }) {
  const { isLoggedIn, isLoading } = useAuth()

  if (isLoading) return <LoadingFallback />
  return isLoggedIn ? children : <Navigate to="/" replace />
}
