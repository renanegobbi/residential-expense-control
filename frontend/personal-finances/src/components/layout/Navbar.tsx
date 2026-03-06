import styled from "styled-components";
import { FiMenu } from "react-icons/fi";

type Props = {
  onHamburger: () => void;
  onToggleSidebar: () => void;
};

export default function Navbar({ onHamburger, onToggleSidebar }: Props) {
  return (
    <Topbar>
      <Left>
        <Hamburger onClick={onHamburger} title="Menu">
          <FiMenu />
        </Hamburger>

        <Brand>
           <span>GESTÃO FINANCEIRA</span>
        </Brand>
      </Left>

      <Right>
        <DesktopOnly>
          <button onClick={onToggleSidebar}>Recolher/Expandir</button>
        </DesktopOnly>
      </Right>
    </Topbar>
  );
}

const Topbar = styled.header`
  height: 56px;
  background: #0f172a; 
  color: white;

  border-bottom: 1px solid rgba(255,255,255,0.08);

  display: flex;
  align-items: center;
  justify-content: space-between;

  padding: 0 16px;

  position: sticky;
  top: 0;
  z-index: 30;
`;

const Left = styled.div`
  display: flex;
  align-items: center;
  gap: 10px;
`;

const Brand = styled.div`
  display: flex;
  gap: 6px;
  align-items: center;

  font-size: 15px;
  font-weight: 600;
  letter-spacing: 0.4px;

  span {
    color: #e5e7eb;
  }
`;

const Right = styled.div`
  display: flex;
  align-items: center;
  gap: 10px;

  button {
    border: 1px solid rgba(255,255,255,0.15);
    background: rgba(255,255,255,0.06);
    color: white;

    padding: 7px 12px;
    border-radius: 10px;

    cursor: pointer;

    transition: all 0.15s ease;
  }

  button:hover {
    background: rgba(255,255,255,0.12);
  }
`;

const Hamburger = styled.button`
  border: 0;
  background: transparent;
  cursor: pointer;
  padding: 8px;
  border-radius: 10px;

  svg {
    font-size: 20px;
  }

  @media (min-width: 900px) {
    display: none;
  }
`;

const DesktopOnly = styled.div`
  display: none;

  @media (min-width: 900px) {
    display: block;
  }
`;

