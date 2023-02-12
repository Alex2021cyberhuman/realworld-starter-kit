export function SmallAvatar({ image, className }: { image?: string | null; className?: string }) {
  return <img className={className} src={image || '/defaultAvatar.png'} alt='#' />;
}
