import styled from "styled-components";
import { NavLink } from "react-router-dom";
import { FiUsers, FiX, FiChevronLeft, FiChevronRight, FiDollarSign, FiGrid } from "react-icons/fi";

type Props = {
  collapsed: boolean;
  mobileOpen: boolean;
  onCloseMobile: () => void;
  onToggleDesktop: () => void;
};

export default function Sidebar({ collapsed, mobileOpen, onCloseMobile, onToggleDesktop }: Props) {
  return (
    <Aside $collapsed={collapsed} $mobileOpen={mobileOpen}>
      <Top>
        <MobileClose onClick={onCloseMobile} title="Fechar">
          <FiX />
        </MobileClose>

        <DesktopToggle onClick={onToggleDesktop} title={collapsed ? "Expandir" : "Recolher"}>
          {collapsed ? <FiChevronRight /> : <FiChevronLeft />}
        </DesktopToggle>
      </Top>

      <Nav>
        <MenuItem to="/people" end onClick={onCloseMobile}>
          <FiUsers />
          {!collapsed && <span>Pessoas</span>}
        </MenuItem>
        <MenuItem to="/transactions" onClick={onCloseMobile}>
          <FiDollarSign />
          {!collapsed && <span>Transações</span>}
        </MenuItem>
        <MenuItem to="/categories" onClick={onCloseMobile}>
          <FiGrid />
          {!collapsed && <span>Categorias</span>}
        </MenuItem>
        <MenuItem to="/totals" onClick={onCloseMobile}>
          <FiGrid />
          {!collapsed && <span>Totais</span>}
        </MenuItem>
      </Nav>
    </Aside>
  );
}

const Aside = styled.aside<{ $collapsed: boolean; $mobileOpen: boolean }>`
  position: fixed;
  top: 56px;
  left: 0;
  height: calc(100vh - 56px);
  width: ${({ $collapsed }) => ($collapsed ? "64px" : "240px")};
  background: #0f172a;
  color: white;
  overflow: hidden;
  z-index: 20;
  transition: width 0.2s ease, transform 0.2s ease;

  @media (max-width: 899px) {
    width: 260px;
    transform: ${({ $mobileOpen }) => ($mobileOpen ? "translateX(0)" : "translateX(-110%)")};
    box-shadow: 0 20px 60px rgba(0,0,0,0.35);
  }
`;

const Top = styled.div`
  height: 44px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 10px;
  background: rgba(255,255,255,0.06);
`;

const MobileClose = styled.button`
  border: 0;
  background: transparent;
  color: white;
  cursor: pointer;
  padding: 8px;
  border-radius: 10px;

  @media (min-width: 900px) {
    display: none;
  }

  svg { font-size: 18px; }
`;

const DesktopToggle = styled.button`
  border: 0;
  background: transparent;
  color: white;
  cursor: pointer;
  padding: 8px;
  border-radius: 10px;

  @media (max-width: 899px) {
    display: none;
  }

  svg { font-size: 18px; }
`;

const Nav = styled.nav`
  padding: 8px;
  display: flex;
  flex-direction: column;
  gap: 6px;
`;

const MenuItem = styled(NavLink)`
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 12px;
  border-radius: 10px;
  color: rgba(255,255,255,0.92);
  text-decoration: none;

  &.active { background: rgba(255,255,255,0.14); }
  &:hover { background: rgba(255,255,255,0.10); }

  span { font-size: 14px; }
`;