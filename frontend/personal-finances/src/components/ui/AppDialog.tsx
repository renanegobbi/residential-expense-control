import styled from "styled-components";

type Props = {
  open: boolean;
  title?: string;
  message: string;
  variant?: "info" | "success" | "warning" | "danger";
  confirmText?: string;
  cancelText?: string;
  showCancel?: boolean;
  onClose: () => void;
  onConfirm?: () => void;
};

export default function AppDialog({
  open,
  title = "Aviso",
  message,
  variant = "info",
  confirmText = "OK",
  cancelText = "Cancelar",
  showCancel = false,
  onClose,
  onConfirm,
}: Props) {
  if (!open) return null;

  const handleConfirm = () => {
    if (onConfirm) onConfirm();
    onClose();
  };

  return (
    <Overlay role="dialog" aria-modal="true" onMouseDown={onClose}>
      <Modal onMouseDown={(e) => e.stopPropagation()} $variant={variant}>
        <Header>
          <Badge $variant={variant} />
          <h3>{title}</h3>
        </Header>

        <Body>{message}</Body>

        <Footer>
          {showCancel && (
            <Btn className="secondary" onClick={onClose}>
              {cancelText}
            </Btn>
          )}

          <Btn
            className={variant === "danger" ? "danger" : "primary"}
            onClick={handleConfirm}
            autoFocus
          >
            {confirmText}
          </Btn>
        </Footer>
      </Modal>
    </Overlay>
  );
}

const Overlay = styled.div`
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.55);
  display: grid;
  place-items: center;
  z-index: 9999;
  padding: 18px;
`;

const Modal = styled.div<{ $variant: NonNullable<Props["variant"]> }>`
  width: min(520px, 96vw);
  background: #fff;
  border-radius: 16px;
  border: 1px solid #e7e9f0;
  box-shadow: 0 30px 80px rgba(0, 0, 0, 0.35);
  overflow: hidden;

  animation: pop 0.15s ease-out;

  @keyframes pop {
    from { transform: translateY(6px); opacity: 0.7; }
    to   { transform: translateY(0); opacity: 1; }
  }
`;

const Header = styled.div`
  display: flex;
  gap: 10px;
  align-items: center;
  padding: 14px 16px;
  border-bottom: 1px solid #eef0f5;

  h3 {
    margin: 0;
    font-size: 16px;
    color: #111827;
  }
`;

const Badge = styled.span<{ $variant: NonNullable<Props["variant"]> }>`
  width: 12px;
  height: 12px;
  border-radius: 999px;

  background: ${({ $variant }) =>
    $variant === "success"
      ? "#16a34a"
      : $variant === "warning"
      ? "#f59e0b"
      : $variant === "danger"
      ? "#ef4444"
      : "#0ea5e9"};
`;

const Body = styled.div`
  padding: 14px 16px;
  color: #374151;
  font-size: 14px;
  line-height: 1.45;
  white-space: pre-wrap;
`;

const Footer = styled.div`
  padding: 14px 16px;
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  border-top: 1px solid #eef0f5;
`;

const Btn = styled.button`
  border: 0;
  padding: 10px 14px;
  border-radius: 12px;
  cursor: pointer;
  font-weight: 800;

  &.primary {
    background: #0ea5e9;
    color: white;
  }
  &.danger {
    background: #ef4444;
    color: white;
  }
  &.secondary {
    background: #6b7280;
    color: white;
  }
`;