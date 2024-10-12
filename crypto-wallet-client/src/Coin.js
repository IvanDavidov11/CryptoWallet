import React from 'react'

const Coin = ({ coin }) => {
    return (
        <li className="coin">
            <p>{coin.name}</p>
            <p>{coin.amount}</p>
            <p>{coin.buyPrice}</p>
        </li>
    )
}

export default Coin