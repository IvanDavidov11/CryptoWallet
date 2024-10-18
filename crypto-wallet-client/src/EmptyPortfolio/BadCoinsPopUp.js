import React from 'react';
import Modal from 'react-modal';

Modal.setAppElement('#root');

const BadCoinsPopUp = ({ openPopUp, setOpenPopUp, badCoins }) => {
  const toggleModal = () => {
    setOpenPopUp(false);
  };

  return (
    <div>
      <Modal
        className="badcoins-popup"
        isOpen={openPopUp}
        onRequestClose={toggleModal}
        overlayClassName="Overlay"
      >
        <h1>Warning: {badCoins.length === 1 ? (`${badCoins.length} Coin`) : (`${badCoins.length} Coins`)} could not be processed.</h1>
        <p>Format of coins has to be NUMBER_OF_COINS|COIN_NAME|BUY_PRICE. If Format is correct, please check the coins' name.</p>
        {badCoins && badCoins.length > 0 ? (
          <ul>
            {badCoins.map((coin) => (
              <li key={coin.id}>
                {coin.name}
              </li>
            ))}
          </ul>
        ) : (
          <p>No bad coins found.</p>
        )}
        <button onClick={toggleModal}>Close</button>
      </Modal>
    </div>
  );
};

export default BadCoinsPopUp;